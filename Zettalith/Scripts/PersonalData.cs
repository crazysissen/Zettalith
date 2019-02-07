﻿using System;
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
            UnlockedPieces = Subpieces.Unlocked,
            SavedPieces = new List<Piece>(),
            SavedSets = new List<Set>()
            {
                new Set(),
            },
            Locked = Achievements.DefaultLocked,
            Unlocked = Achievements.DefaultUnlocked,
        };

        public Settings CurrentSettings { get; set; }

        public bool[] UnlockedPieces { get; set; }
        //public List<SubPiece> SavedSubPieces { get; set; }
        public List<Piece> SavedPieces { get; set; }
        public List<Set> SavedSets { get; set; }

        public Dictionary<string, Achievement> Locked { get; set; }
        public Dictionary<string, Achievement> Unlocked { get; set; }
    }
}