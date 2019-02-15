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
    class LowHealthLowAtk : Middle
    {
        public LowHealthLowAtk()
        {
            Name = "Low Health, Low Atk";
            Health = 3;
            AttackDamage = 1;
            ManaCost = new Mana(0, 1, 0);
            Description = "Trash Body.";
            Texture = Load.Get<Texture2D>("LowATKLowHPMiddle");
        }
    }
}
