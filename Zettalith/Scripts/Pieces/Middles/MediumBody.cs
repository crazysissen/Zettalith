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
            Name = "Infantry";
            Health = 6;
            AttackDamage = 3;
            ManaCost = new Mana(2, 2, 0);
            Description = "Mediocre body with decent offense";
            Texture = Load.Get<Texture2D>("MediumMiddle");
        }
    }
}
