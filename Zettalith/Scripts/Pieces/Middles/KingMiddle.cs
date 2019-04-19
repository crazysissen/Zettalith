﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class KingMiddle : Middle
    {
        public KingMiddle()
        {
            Name = "King Middle";
            Health = 1;
            AttackDamage = 2;
            Texture = Load.Get<Texture2D>("SixtenBottom3");
        }
    }
}
