using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
{
    [Serializable]
    struct PersonalData
    {
        List<SubPiece> savedSubPieces;
        List<Piece> savedPieces;
        List<Army> savedArmies;

        public PersonalData(List<SubPiece> subPieces, List<Piece> pieces, List<Army> armies)
        {
            savedSubPieces = subPieces;
            savedPieces = pieces;
            savedArmies = armies;
        }
    }
}
