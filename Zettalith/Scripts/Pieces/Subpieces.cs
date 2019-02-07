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
        public static List<System.Type> subpieces = new List<System.Type>
        {
            typeof(TestTop1), typeof(TestTop2), //typeof(Top2),
            typeof(TestMiddle1), typeof(TestMiddle2), //typeof(Middle2),
            typeof(TestBottom1), typeof(TestBottom2) //typeof(Bottom2)
        };

        // Bool values here decides if a subpiece is unlocked or not
        // NOTE! Order corresponds to the above subpieces list
        public static bool[] Unlocked = new bool[]
        {
            true, true, /*Tops*/
            true, true, /*Middles*/
            true, true, /*Bottoms*/
        };

        // Creates a SubPiece from a selected index (in Subpieces.subpieces list)
        public static SubPiece FromIndex(int index)
        {
            return (SubPiece)Activator.CreateInstance(subpieces[index]);
        }

        public static List<T> GetSubpieces<T>() where T : SubPiece
        {
            List<T> tempList = new List<T>();

            for (int i = 0; i < subpieces.Count; ++i)
            {
                if (PersonalData.UserData.UnlockedPieces[i])
                {
                    tempList.Add(FromIndex(i) as T);
                }
            }

            return tempList;
        }
    }
}
