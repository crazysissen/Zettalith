using System;
using System.Collections.Generic;

namespace Zettalith.Pieces
{
    [Serializable]
    class Set
    {
        public List<Piece> Pieces { get; private set; }

        public string Name { get; private set; } = "Unnamed Collection";
        public bool Complete => Pieces.Count == MaxSize;

        int MaxSize { get; set; } = 20;

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

        public void NameCollection(string name)
        {
            Name = name;
        }
    }
}
