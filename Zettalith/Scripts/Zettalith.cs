using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    class Zettalith
    {
        public Mana ManaCost => top.ManaCost + middle.ManaCost + bottom.ManaCost;

        Top top;
        Middle middle;
        Bottom bottom;

        public Zettalith(Top top, Middle middle, Bottom bottom)
        {
            this.top = top;
            this.middle = middle;
            this.bottom = bottom;
        }
    }
}
