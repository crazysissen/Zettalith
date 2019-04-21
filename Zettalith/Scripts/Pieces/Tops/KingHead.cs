using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class KingHead : Top
    {
        public KingHead()
        {
            Name = "King Head";
            Health = 1;
            AttackDamage = 1;
            Texture = Load.Get<Texture2D>("AltKing");

            Description = "The finest crown in the entire kingdom";
        }
    }
}
