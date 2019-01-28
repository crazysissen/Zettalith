using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class Multiplication : Modifier
    {
        public Stats StatChanges { get; set; } = new Stats();

        public Multiplication(Piece owner, Stats changes, bool permanent) : base(owner, permanent)
        {
            StatChanges = changes;
        }
    }
}