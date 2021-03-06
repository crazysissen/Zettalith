﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class HighHealthHighAtk : Middle
    {
        public HighHealthHighAtk()
        {
            Name = "Gladiator";
            Health = 10;
            AttackDamage = 5;
            ManaCost = new Mana(0, 0, 6);
            Description = "Strong body with good offense";
            Texture = Load.Get<Texture2D>("HighATKHighHPMiddle");
        }
    }
}
