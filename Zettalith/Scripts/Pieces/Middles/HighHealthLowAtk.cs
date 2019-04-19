using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            Description = "Good Health.";
            Texture = Load.Get<Texture2D>("LowATKHighHPMiddle");
        }
    }
}
