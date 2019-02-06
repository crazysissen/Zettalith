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
    class TestTop2 : Top
    {
        TestTop2()
        {
            Name = "TestTop2";
            Health = 20;
            AttackDamage = 3;
            ManaCost = new Mana(0, 4, 0);
            Description = "Strong but not very powerful head.";
            Texture = Load.Get<Texture2D>("TestSubpiece");
        }
    }
}
