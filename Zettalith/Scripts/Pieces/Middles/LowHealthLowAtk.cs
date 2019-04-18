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
            Health = 4;
            AttackDamage = 2;
            ManaCost = new Mana(2, 0, 0);
            Description = "Trash Body.";
            Texture = Load.Get<Texture2D>("LowATKLowHPMiddle");
        }
    }
}
