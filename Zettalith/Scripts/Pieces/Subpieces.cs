﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
{
    static class Subpieces
    {
        // TODO: Add all SubPieces to exist in the game to this list with the format below
        public static List<Type> SubPieces = new List<Type>
        {
            typeof(SingleTarget), typeof(Bomb), typeof(Pyro), typeof(Cyclops),
            typeof(MediumBody), typeof(LowHealthLowAtk), typeof(LowHealthHighAtk), typeof(HighHealthLowAtk), typeof(HighHealthHighAtk),
            typeof(Straight), typeof(Diagonal), typeof(Teleporter), typeof(Queen), typeof(Diagonal2), typeof(Straight2), typeof(Teleporter2), typeof(Queen2), typeof(Healer), typeof(Lob),
            typeof(KingHead), typeof(KingFeet), typeof(KingMiddle),
            typeof(Swap), typeof(Cone), typeof(CyclopsNightmare), typeof(CyclopsAbomination), typeof(LobHealer), typeof(ConeHealer), typeof(OneTile), 
        };

        // Declares the desired default pieces for the default deck
        public static Type DefaultTop = typeof(SingleTarget);
        public static Type DefaultMiddle = typeof(MediumBody);
        public static Type DefaultBottom = typeof(Straight);

        // Bool values here decides if a subpiece is unlocked or not
        // NOTE! Order corresponds to the above subpieces list
        public static bool[] Unlocked = new bool[]
        {
            true, true, true,
            true, true, true, true, true,
            true, true, true, true, true, true, true, true, true, true, true,
            false, false, false,
            true, true, true, true, true, true, true
        };

        // Creates a SubPiece from a selected index (in Subpieces.subpieces list)
        public static SubPiece FromIndex(int index)
        {
            return (SubPiece)Activator.CreateInstance(SubPieces[index]);
        }

        public static List<T> GetSubpieces<T>() where T : SubPiece
        {
            List<T> tempList = new List<T>();

            SaveLoad.Load();

            for (int i = 0; i < SubPieces.Count; ++i)
            {
                if (PersonalData.UserData.UnlockedPieces[i] && SubPieces[i].BaseType == typeof(T))
                {
                    tempList.Add(FromIndex(i) as T);
                }
            }

            return tempList;
        }
    }
}
