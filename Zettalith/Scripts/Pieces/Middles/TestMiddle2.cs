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
    class TestMiddle2 : Middle
    {
        public TestMiddle2()
        {
            Name = "TestMiddle2";
            Health = 20;
            AttackDamage = 3;
            ManaCost = new Mana(0, 3, 0);
            Description = "Sturdy but not very powerful body.";
            Texture = Load.Get<Texture2D>("TestSubpiece2");
        }
    }
}
