using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
{
    [Serializable]
    abstract class Modifier
    {
        public Stats StatChanges { get; set; } = new Stats();

        public virtual bool Permanent { get; protected set; }
        //public bool Addition { get; private set; }
        //public List<string> DirectModifiers { get; set; } = new List<string>();

        public Modifier(bool permanent)
        {
            Permanent = permanent;
        }
    }
}