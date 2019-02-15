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
    class MediumBody : Middle
    {
        public MediumBody()
        {
            Name = "Medium Body";
            Health = 5;
            AttackDamage = 2;
            ManaCost = new Mana(1, 1, 1);
            Description = "Balanced body.";
            Texture = Load.Get<Texture2D>("MediumMiddle");
        }
    }
}
