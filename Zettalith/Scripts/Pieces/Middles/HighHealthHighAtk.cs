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
    class HighHealthHighAtk : Middle
    {
        public HighHealthHighAtk()
        {
            Name = "High Health, High Atk";
            Health = 7;
            AttackDamage = 3;
            ManaCost = new Mana(2, 1, 2);
            Description = "This body is better than the others.";
            Texture = Load.Get<Texture2D>("HighATKHighHPMiddle");
        }
    }
}
