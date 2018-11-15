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
            
        }
    }
}
