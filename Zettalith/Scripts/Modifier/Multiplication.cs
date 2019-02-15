using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    [Serializable]
    class Multiplication : Modifier
    {
        public Stats StatChanges { get; set; } = new Stats();

        public Multiplication(Stats changes, bool permanent) : base(permanent)
        {
            StatChanges = changes;
        }
    }
}