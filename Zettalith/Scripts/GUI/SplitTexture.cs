using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class SplitTexture
    {
        public Texture2D Texture { get; private set; }
        public Texture2D[,] Components { get; private set; }

        public void SetTexture(Texture2D texture, int topMargin, int rightMargin, int bottomMargin, int leftMargin)
        {
            GraphicsDevice g = XNAController.Graphics.GraphicsDevice;

            Texture = texture;
            Components = new Texture2D[3, 3];
            //{
            //    { new Texture2D(g, ) }
            //}

            Point[,] originPoints = new Point[,]
            {
                {
                    new Point(leftMargin - 1, topMargin - 1),
                    new Point(leftMargin, topMargin - 1),
                    new Point(texture.Width - rightMargin, topMargin - 1)
                },

                {
                    new Point(leftMargin - 1, leftMargin),
                    new Point(leftMargin, topMargin),
                    new Point(texture.Width - rightMargin, topMargin)
                },

                {
                    new Point(leftMargin - 1, texture.Height - bottomMargin),
                    new Point(leftMargin, texture.Height - bottomMargin),
                    new Point(texture.Width - rightMargin, texture.Height - bottomMargin)
                }
            };

            //for (int x = 0; x < texture.Width; x++)
            //{
            //    for (int y = 0; y < texture.Height; y++)
            //    {

            //    }
            //}

            Components = new Texture2D[3,3];
        }
    }
}
