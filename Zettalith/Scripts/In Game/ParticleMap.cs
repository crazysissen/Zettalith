using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    class ParticleMap
    {
        public Renderer.SpriteScreenFloating[] Renderers => renderers;
        public GUI.Collection Collection { get; set; }

        private Renderer.SpriteScreenFloating[] renderers;
        private Texture2D[] textures;
        private int count, width, height, textureHeight;
        //private Point previousTL, previousBR;

        private List<Particle> particles;
        private ConvertibleGrid<Color>[] grids;
        private Color GetGrid(int x, int y)
        {
            try
            {
                return grids[y / textureHeight][x, y % textureHeight];
            }
            catch
            {
                return Color.Red;
            }
        }

        private Point offset;

        private void SetGrid(int x, int y, Color color)
        {
            try
            {
                grids[y / textureHeight][x, y % textureHeight] = color;
            }
            catch { }
        }

        public ParticleMap(GUI.Collection mainCollection, int width, int height, MainLayer mainLayer, int minLayer, int nextMaxLayer, bool skipOdd)
        {
            this.width = width;
            this.height = height;
            count = 0;
            particles = new List<Particle>();

            List<int> layers = new List<int>();

            for (int i = minLayer; i < nextMaxLayer; i++)
            {
                if (skipOdd && (i % 2 == 1) || (i % 2 == -1))
                {
                    continue;
                }

                count++;
                layers.Add(i);
            }

            textureHeight = height / count;

            Collection = new GUI.Collection();
            mainCollection.Add(Collection);

            textures = new Texture2D[count];

            for (int i = 0; i < count; i++)
            {
                textures[i] = new Texture2D(XNAController.Graphics.GraphicsDevice, width, height / count);
            }

            renderers = new Renderer.SpriteScreenFloating[count];
            int currentLayer = -1;

            for (int i = 0; i < count; i++)
            {
                renderers[i] = new Renderer.SpriteScreenFloating(new Layer(mainLayer, layers[++currentLayer]), textures[i]/*, Load.Get<Texture2D>("Square")*/, Vector2.Zero, Vector2.One, Color.White/*Color.Lerp(Color.DarkGray, Color.DarkBlue, (float)i / count)*/, 0, Vector2.Zero, SpriteEffects.None);
                Collection.Add(renderers[i]);
            }
        }

        public void Update(Vector2 coordinateTopLeft, Vector2 coordinateBottomRight, bool worldSpace, float deltaTime)
        {
            Camera camera = RendererController.Camera;
            Vector2 topLeft = coordinateTopLeft, bottomRight = coordinateBottomRight;

            grids = new ConvertibleGrid<Color>[count];

            if (worldSpace)
            {
                topLeft = camera.WorldToScreenPosition(coordinateTopLeft);
                bottomRight = camera.WorldToScreenPosition(coordinateBottomRight);
            }

            float stepDistance = (bottomRight.Y - topLeft.Y) / count;

            Vector2 scale = new Vector2((bottomRight.X - topLeft.X) / width, (bottomRight.Y - topLeft.Y) / height);

            for (int i = 0; i < count; i++)
            {
                grids[i] = new ConvertibleGrid<Color>(width, textureHeight);

                renderers[i].Position = topLeft + new Vector2(0, stepDistance * i);
                renderers[i].Size = scale;
            }

            Queue<Particle> removeQueue = new Queue<Particle>();

            foreach (Particle particle in particles)
            {
                if (particle.Destroyed)
                {
                    //removeQueue.Enqueue(particle);
                    continue;
                }

                particle.Update(deltaTime);

                Point point = (((camera.WorldToScreenPosition(particle.Position) - topLeft) / (bottomRight - topLeft)) * new Vector2(width, height)).RoundToPoint();

                if (point.X < 0 || point.X >= width || point.Y < 0 || point.Y >= height)
                {
                    particle.Destroy();
                    continue;
                }

                SetGrid(point.X, point.Y, particle.GetColor());
            }

            //while (removeQueue.Count > 0)
            //{
            //    particles.Remove(removeQueue.Dequeue());
            //}

            for (int i = 0; i < count; i++)
            {
                textures[i].SetData(grids[i].array);
                renderers[i].Texture = textures[i];
            }
        }

        public void Destroy()
        {
            particles.Clear();
        }

        // Premades

        public void Beam(Vector2 startPosition, Vector2 endPosition, Color startColor, Color endColor, float density = 100.0f, float speed = 1.0f)
        {
            Random r = new Random();

            float length = (endPosition - startPosition).Length();
            Vector2 step = (endPosition - startPosition) / (density * length);
            int steps = (int)(density * length);

            for (int i = 0; i < steps; i++)
            {
                ParticleStraight newParticle = new ParticleStraight(1, startPosition + step * i, speed * 0.5f * new Vector2((float)r.NextDouble() * 2 - 1, (float)r.NextDouble() * 2 - 1));
                particles.Add(newParticle);
                newParticle.Start(1, startColor, endColor);
            }
        }

        public void Destroy(TileObject piece, float force)
        {
            Texture2D texture = piece.Renderer.Texture;
            Color[] data = new Color[texture.Height * texture.Width];

            texture.GetData(data);

            Vector2 origin = new Vector2(piece.Renderer.Position.X - (13f / 30), piece.Renderer.Position.Y - piece.Renderer.Texture.Height / 30f + (9f / 30));

            Random r = new Random();

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    Color color = data[x + y * texture.Width];

                    if (color.A != 0)
                    {
                        ParticleStraight newParticle = new ParticleStraight(1, origin + new Vector2(x, y) / 32f, 0.5f * new Vector2((float)r.NextDouble() * 2 - 1, (float)r.NextDouble() * 2 - 1) - new Vector2(0.3f, 0.2f));
                        particles.Add(newParticle);
                        newParticle.Start(1, color, new Color(color, 0.0f));
                    }
                }
            }
        }

        private class Particle
        {
            public int Pixels { get; private set; }
            public Vector2 Position { get; set; }

            public bool Destroyed { get; private set; }

            protected Color startColor, endColor;
            float currentTime, endTime;

            public Particle(int pixels, Vector2 position)
            {
                Pixels = pixels;
                Position = position;
            }

            public void Start(float time, Color startColor, Color endColor)
            {
                endTime = time;
                this.endColor = endColor;
                this.startColor = startColor;
            }

            public Color GetColor()
            {
                return Color.Lerp(startColor, endColor, currentTime / endTime);
            }

            public void Destroy()
            {
                Destroyed = true;
            }

            public virtual void Update(float deltaTime)
            {
                if (endTime != 0)
                {
                    currentTime += deltaTime;

                    if (currentTime >= endTime)
                    {
                        Destroy();
                    }
                }
            }
        }

        private class ParticleStraight : Particle
        {
            public Vector2 Velocity { get; set; }

            public ParticleStraight(int pixels, Vector2 position) : base(pixels, position)
            {

            }

            public ParticleStraight(int pixels, Vector2 position, Vector2 velocity) : base(pixels, position)
            {
                Velocity = velocity;
            }

            public override void Update(float deltaTime)
            {
                base.Update(deltaTime);

                Position += Velocity * deltaTime;
            }
        }
    }
}
