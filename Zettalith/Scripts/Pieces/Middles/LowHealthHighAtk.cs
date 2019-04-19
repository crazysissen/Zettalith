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
    class LowHealthHighAtk : Middle
    {
        public LowHealthHighAtk()
        {
            Name = "Low Health, High Atk";
            Health = 4;
            AttackDamage = 5;
            ManaCost = new Mana(4, 0, 0);
            Description = "Good Atk.";
            Texture = Load.Get<Texture2D>("HighATKLowHPMiddle");
        }
    }
}
