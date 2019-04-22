using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class WeakShield : Middle
    {
        public WeakShield()
        {
            Name = "Paladin";
            Health = 2;
            Armor = 4;
            AttackDamage = 1;
            ManaCost = new Mana(2, 0, 0);
            Description = "Weakly shielded body";
            Texture = Load.Get<Texture2D>("WeakShieldBody");
        }
    }
}
