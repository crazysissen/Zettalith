using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    class KingMiddle : Middle
    {
        public KingMiddle()
        {
            Name = "King Middle";
            Health = 1;
            Texture = Load.Get<Texture2D>("SixtenBottom3");
        }
    }
}
