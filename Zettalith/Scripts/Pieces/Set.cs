using System;
using System.Collections.Generic;

namespace Zettalith
{
    [Serializable]
    class Set
    {
        public List<Piece> Pieces { get; set; } = new List<Piece>();

        public string Name { get; set; } = "Default Set";
        public bool Complete => Pieces.Count == MaxSize;

        public static int MaxSize { get; private set; } = 20;
        public int Size { get; set; } = 20;

        public void AddUnit(Piece unit)
        {
            if (!Complete)
            {
                Pieces.Add(unit);
            }
            else
            {
                // DECK IS FULL REEEEEEEEEEEEEEEEEEE
            }
        }
    }
}
