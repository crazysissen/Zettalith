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
        // TODO: Add all SubPieces to exist in the game to this list with the format below
        public static List<Type> SubPieces = new List<Type>
        {
            typeof(Bomb), typeof(Pyro), typeof(Cyclops), /*typeof(Duplicator),*/ //typeof(Top2),
            typeof(LowHealthLowAtk), typeof(LowHealthHighAtk), typeof(HighHealthLowAtk), typeof(HighHealthHighAtk), typeof(MediumBody), //typeof(Middle2),
            typeof(Diagonal), typeof(Straight), typeof(Teleporter), typeof(Queen), typeof(Diagonal2), typeof(Straight2), typeof(Teleporter2), typeof(Queen2), typeof(SingleTarget), typeof(Healer), typeof(Lob), typeof(KingHead), typeof(KingFeet), //typeof(Bottom2)
        };

        // Bool values here decides if a subpiece is unlocked or not
        // NOTE! Order corresponds to the above subpieces list
        public static bool[] Unlocked = new bool[]
        {
            true, true, true, /*Tops*/
            true, true, true, true, true, /*Middles*/
            true, true, true, true, true, true, true, true, true, true, true, false, false, /*Bottoms*/
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
