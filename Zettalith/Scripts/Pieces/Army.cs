using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    [Serializable]
    class Army
    {
        public List<Piece> army;

        public bool Complete => army.Count == maxSize;

        int maxSize = 20;
    }
}
