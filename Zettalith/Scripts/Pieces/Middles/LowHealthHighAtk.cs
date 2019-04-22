using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class LowHealthHighAtk : Middle
    {
        public LowHealthHighAtk()
        {
            Name = "Mercenary";
            Health = 4;
            AttackDamage = 5;
            ManaCost = new Mana(4, 0, 0);
            Description = "Weak body with good offense";
            Texture = Load.Get<Texture2D>("HighATKLowHPMiddle");
        }
    }
}
