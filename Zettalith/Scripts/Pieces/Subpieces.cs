using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
{
    static class Subpieces
    {
        public static List<System.Type> subpieces = new List<System.Type>
        {
            typeof(Top1), //typeof(Top2),
            typeof(Middle1), //typeof(Middle2),
            typeof(Bottom1) //typeof(Bottom2)
        };
    }
}
