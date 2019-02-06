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
    class TestBottom1 : Bottom
    {
        TestBottom1()
        {
            Name = "TestBottom1";
            Health = 10;
            AttackDamage = 7;
            ManaCost = new Mana(5, 0, 0);
            Description = "Weak but strong head.";
            Texture = Load.Get<Texture2D>("TestSubpiece");
        }
    }
}
