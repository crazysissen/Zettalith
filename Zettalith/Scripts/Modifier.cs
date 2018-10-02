using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
{
    class Modifier
    {
        public Stats StatChanges { get; set; } = new Stats();
        public List<string> DirectModifiers { get; set; } = new List<string>();
    }
}