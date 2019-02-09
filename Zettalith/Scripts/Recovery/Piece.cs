using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
{
    [Serializable]
    class Piece
    {
        public byte TopIndex { get; set; }
        public byte MiddleIndex { get; set; }
        public byte BottomIndex { get; set; }

        public Piece(byte topIndex, byte middleIndex, byte bottomIndex)
        {
            TopIndex = topIndex;
            MiddleIndex = middleIndex;
            BottomIndex = bottomIndex;
        }

        //public Top Top { get; set; }
        //public Middle Middle { get; set; }
        //public Bottom Bottom { get; set; }

        //public Piece(Top top, Middle middle, Bottom bottom)
        //{
        //    this.Top = top;
        //    this.Middle = middle;
        //    this.Bottom = bottom;
        //}

        //Addition statChange = new Addition(null, new Stats(0));

        //public Modifier CumulativeModifier
        //{
        //    get
        //    {
        //        Modifier cumulative = new Modifier();

        //        foreach (Modifier modifier in modifiers)
        //        {
        //            cumulative.StatChanges += modifier.StatChanges;

        //            foreach (string mod in modifier.DirectModifiers)
        //            {
        //                if (!cumulative.DirectModifiers.Contains(mod))
        //                    cumulative.DirectModifiers.Add(mod);
        //            }
        //        }

        //        return cumulative;
        //    }
        //}

        //public Stats ModifiedStats => BaseStats + CumulativeModifier.StatChanges;

        //List<Modifier> modifiers = new List<Modifier>();
    }
}