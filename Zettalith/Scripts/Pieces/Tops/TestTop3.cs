using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    class TestTop3 : Top
    {
        public TestTop3()
        {
            Name = "Woop";
            ManaCost = new Mana(4, 2, 0);
            Texture = Load.Get<Texture2D>("TestSubpiece");
        }
    }
}
