using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class CloudManager
    {
        public const float
            DEPTHSLOW = 0.5f;

        Texture2D[] cloudTextures;

        float currentTime, multiplier, speed, inaccuracy, spriteScale, originX;
        int rows;
        Vector2 topLeft, bottomRight, scale;

        Noise noise;
        Random random;

        List<Cloud> clouds;

        public CloudManager(int rows, float speed, Vector2 topLeft, Vector2 bottomRight, float multiplier, float inaccuracy, Vector2 scale, float spriteScale, float originX)
        {
            this.rows = rows;
            this.multiplier = multiplier;
            this.speed = speed;
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
            this.inaccuracy = inaccuracy;
            this.scale = scale;
            this.spriteScale = spriteScale;
            this.originX = originX;

            noise = new Noise();
            random = new Random();

            clouds = new List<Cloud>();

            cloudTextures = new Texture2D[]
            {
                Load.Get<Texture2D>("Cloud1"),
                Load.Get<Texture2D>("Cloud2"),
                Load.Get<Texture2D>("Cloud3")
            };
        }

        public void Update(float deltaTime)
        {
            currentTime += deltaTime;
            float step = (bottomRight.Y - topLeft.Y) / rows;

            for (int i = 0; i < rows; i++)
            {
                float currentY = topLeft.Y + i * step;

                float value = noise.Generate(currentTime * scale.X * speed, currentY * scale.Y) * 0.5f + 0.5f,
                    value2 = value * value/* * value*/;
                bool spawn = random.NextDouble() < (value2 /** 1.05f - 0.05f*/).Min(0) * multiplier * deltaTime;

                if (spawn)
                {
                    Vector2 offset = new Vector2();
                    if (inaccuracy != 0)
                    {
                        offset = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f) * 2 * inaccuracy;
                    }

                    Cloud newCloud = new Cloud(new Vector2(topLeft.X + offset.X, currentY + offset.Y), (bottomRight.Y - currentY) / bottomRight.Y);

                    Texture2D texture = GetTexture(value, out int index);
                    newCloud.Renderer = new Renderer.Sprite(new Layer(MainLayer.Background, -1000 + (int)(newCloud.Position.Y * 20)), texture, newCloud.Position, Vector2.One * spriteScale, Color.White, 0, new Vector2(texture.Width * 0.5f, texture.Height), random.Next(2) == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
                    newCloud.Renderer.OnRender += newCloud.OnRender;
                    newCloud.XOrigin = originX;

                    clouds.Add(newCloud);
                }
            }

            Queue<Cloud> destroyClouds = new Queue<Cloud>();
            foreach (Cloud cloud in clouds)
            {
                cloud.Position += new Vector2(deltaTime * speed, 0);

                if (cloud.Position.X > bottomRight.X)
                {
                    destroyClouds.Enqueue(cloud);
                }
            }

            while (destroyClouds.Count > 0)
            {
                Cloud cloud = destroyClouds.Dequeue();
                cloud.Destroy();
                clouds.Remove(cloud);
            }
        }

        public void FastForward(int steps, float step)
        {
            for (int i = 0; i < steps; i++)
            {
                Update(step);
            }
        }

        Texture2D GetTexture(float weight, out int index)
        {
            index = (int)(3 * Math.Pow(random.NextDouble(), 1 / (1.5 * weight + 0.5)));

            return cloudTextures[index];
        }

        class Cloud
        {
            const float
                MODIFIER = 1.2f;

            public Renderer.Sprite Renderer { get; set; }
            public float Depth { get; set; }
            public float XOrigin { get; set; }
            public Vector2 Position { get; set; }

            public Cloud(Vector2 position, float depth)
            {
                Position = position;
                Depth = depth;
            }

            public void OnRender()
            {
                Renderer.Position = (Position + new Vector2(((RendererController.Camera.Position.X - XOrigin) * MODIFIER * Depth), 0)) * new Vector2((1 - (0.3f * Depth)), 1);
            }

            public void Destroy()
            {
                Renderer.Destroy();
            }
        }
    }
}
