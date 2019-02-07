using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class ImageProcessing
    {
        public const int
            PIECEWIDTH = 26,
            FACEHEIGHT = 18;

        public static Texture2D CombinePiece(Texture2D bottom, Texture2D middle, Texture2D top)
        {
            Texture2D newTexture = new Texture2D(XNAController.Graphics.GraphicsDevice, PIECEWIDTH, bottom.Height + middle.Height + top.Height - FACEHEIGHT * 2);
            

            //for (int x = 0; x < newTexture; x++)
            //{

            //}

            return newTexture;
        }
    }
}
