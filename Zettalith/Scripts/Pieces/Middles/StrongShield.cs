using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class StrongShield : Middle
    {
        public StrongShield()
        {
            Name = "Strong Shield";
            Health = 5;
            Armor = 7;
            AttackDamage = 1;
            ManaCost = new Mana(0, 4, 0);
            Description = "Strongly shielded body";
            Texture = Load.Get<Texture2D>("StrongShieldBody");
        }
    }
}
