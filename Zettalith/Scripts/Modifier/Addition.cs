using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    [Serializable]
    class Addition : Modifier
    {
        public Stats StatChanges { get; set; } = new Stats();

        public Addition(Stats changes, bool permanent) : base(permanent)
        {
            StatChanges = changes;
        }
    }
}