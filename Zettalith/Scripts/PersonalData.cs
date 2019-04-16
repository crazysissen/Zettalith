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
        public static Settings Settings => UserData.CurrentSettings;
        public static PersonalData UserData { get; set; } = new PersonalData();

        public static PersonalData Default = new PersonalData
        {
            CurrentSettings = new Settings(),
            UnlockedPieces = Subpieces.Unlocked.ToList(),
            SavedPieces = new List<Piece>(),
            SavedSets = new List<Set>() /*{ CreateDefaultSet() }*/,
            Locked = Achievements.DefaultLocked,
            Unlocked = Achievements.DefaultUnlocked,
        };

        public Settings CurrentSettings { get; set; }

        public List<bool> UnlockedPieces { get; set; }
        //public List<SubPiece> SavedSubPieces { get; set; }
        public List<Piece> SavedPieces { get; set; }
        public List<Set> SavedSets { get; set; }

        public Dictionary<string, Achievement> Locked { get; set; }
        public Dictionary<string, Achievement> Unlocked { get; set; }

        //public static Set CreateDefaultSet()
        //{
        //    Set set = new Set();

        //    List<Top> tops = Subpieces.GetSubpieces<Top>();
        //    List<Middle> middles = Subpieces.GetSubpieces<Middle>();
        //    List<Bottom> bottoms = Subpieces.GetSubpieces<Bottom>();

        //    for (int i = 0; i < Set.MaxSize; ++i)
        //    {
        //        set.AddUnit(new Piece((byte)Subpieces.SubPieces.IndexOf(tops[0].GetType()), (byte)Subpieces.SubPieces.IndexOf(middles[0].GetType()), (byte)Subpieces.SubPieces.IndexOf(bottoms[0].GetType())));
        //    }

        //    return set;
        //}
    }
}