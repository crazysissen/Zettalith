using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class HighHealthLowAtk : Middle
    {
        public HighHealthLowAtk()
        {
            Name = "High Health, Low Atk";
            Health = 10;
            AttackDamage = 1;
            ManaCost = new Mana(0, 4, 0);
            Description = "Strong body with low offense";
            Texture = Load.Get<Texture2D>("LowATKHighHPMiddle");
        }
    }
}
