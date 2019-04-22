using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class LowHealthLowAtk : Middle
    {
        public LowHealthLowAtk()
        {
            Name = "Peasant";
            Health = 4;
            AttackDamage = 1;
            ManaCost = new Mana(2, 0, 0);
            Description = "Weak body with low offense";
            Texture = Load.Get<Texture2D>("LowATKLowHPMiddle");
        }
    }
}
