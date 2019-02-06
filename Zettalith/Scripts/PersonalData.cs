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
        public static PersonalData UserData { get; set; } = new PersonalData();

        public bool[] UnlockedPieces => Subpieces.Unlocked;
        //public List<SubPiece> SavedSubPieces { get; set; }
        public List<Piece> SavedPieces { get; set; }
        public List<Set> SavedSets { get; set; }

        public Dictionary<string, Achievement> Locked { get; set; }
        public Dictionary<string, Achievement> Unlocked { get; set; }
    }
}