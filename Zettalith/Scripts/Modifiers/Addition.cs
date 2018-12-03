using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class Addition : Modifier
    {
        public Stats StatChanges { get; set; } = new Stats();

        public Addition(Piece owner, Stats changes, bool permanent) : base(owner, permanent)
        {
            StatChanges = changes;
        }
    }
}
