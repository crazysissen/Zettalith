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
        public List<SubPiece> SavedSubPieces { get; set; }
        public List<Piece> SavedPieces { get; set; }
        public List<Set> SavedSets { get; set; }

        public Dictionary<string, Achievement> Locked { get; set; }
        public Dictionary<string, Achievement> Unlocked { get; set; }
    }
}