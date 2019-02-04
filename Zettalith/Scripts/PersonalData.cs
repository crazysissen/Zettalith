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
        List<Collection> savedArmies;

        public PersonalData(List<SubPiece> subPieces, List<Piece> pieces, List<Collection> collections)
        {
            savedSubPieces = subPieces;
            savedPieces = pieces;
            savedArmies = collections;
        }
    }
}
