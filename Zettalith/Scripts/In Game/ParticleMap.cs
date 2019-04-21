//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;

//namespace Zettalith
//{
//    class ParticleMap
//    {
//        public Renderer.SpriteScreenFloating[] Renderers => renderers;
//        public GUI.Collection Collection { get; set; }

//        private Renderer.SpriteScreenFloating[] renderers;
//        private Texture2D[] textures;
//        private int count;
//        private Point previousTL, previousBR;

//        public ParticleMap(int width, int height, MainLayer mainLayer, int minLayer, int nextMaxLayer)
//        {
//            SetSize(width, height);
//            count = nextMaxLayer - minLayer;

//            Collection = new GUI.Collection();

//            for (int i = 0; i < count; i++)
//            {
//                textures[i] = new Texture2D(XNAController.Graphics.GraphicsDevice, width, height / count);
//            }

//            renderers = new Renderer.SpriteScreenFloating[count];
//            int currentLayer = minLayer - 1;

//            for (int i = 0; i < count; i++)
//            {
//                renderers[i] = new Renderer.SpriteScreenFloating(new Layer(mainLayer, ++currentLayer), textures[i], Vector2.Zero, Vector2.One, Color.White, 0, Vector2.Zero, SpriteEffects.None);
//            }
//        }

//        public void Update(Vector2 coordinateTopLeft, Vector2 coordinateBottomRight, bool worldSpace)
//        {
//            Camera camera = RendererController.Camera;
//            Vector2 topLeft = coordinateTopLeft, bottomRight = coordinateBottomRight;

//            if (worldSpace)
//            {
//                topLeft = camera.WorldToScreenPosition(coordinateTopLeft);
//                bottomRight = camera.WorldToScreenPosition(coordinateBottomRight);
//            }

//            float stepDistance = (bottomRight.Y - topLeft.Y) / count;

//            for (int i = 0; i < count; i++)
//            {
//                renderers[i].Transform.po
//            }
//        }

//        public void SetSize(int x, int y)
//        {

//        }

//        // Premades

//        public void Beam(Vector2 startPosition, Vector2 endPosition, Color startColor, Color endColor, float density = 1, )
//        {

//        }

//        private class Particle
//        {
//            public int Pixels { get; private set; }
//            public Vector2 Position { get; set; }

//            public Particle()
//            {

//            }
//        }
//    }
//}
