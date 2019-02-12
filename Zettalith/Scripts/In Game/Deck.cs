using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class Deck
    {
        public Piece[] Pieces { get; private set; }

        Stack<int> order;

        public Deck(Piece[] pieces)
        {
            order = new Stack<int>();
        }
    }
}
