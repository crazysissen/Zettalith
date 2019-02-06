using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
{
    struct PersonalData
    {
        public static List<SubPiece> savedSubPieces { get; set; }
        public static List<Piece> savedPieces { get; set; }
        public static List<Set> savedSets { get; set; }
    }
}