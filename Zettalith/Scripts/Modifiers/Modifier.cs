using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
{
    abstract class Modifier
    {
        public Piece Owner { get; protected set; }
        //public Stats StatChanges { get; set; } = new Stats();

        public virtual bool Permanent { get; protected set; }
        //public bool Addition { get; private set; }
        //public List<string> DirectModifiers { get; set; } = new List<string>();

        public Modifier(Piece owner, bool permanent)
        {
            Owner = owner;
            Permanent = permanent;
        }
    }
}