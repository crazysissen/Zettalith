using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    [Serializable]
    abstract class SubPiece
    {
        //public int Index { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int AttackDamage { get; set; }
        public Mana ManaCost { get; set; }
        public Mana MoveCost { get; set; }
        public Ability Ability { get; set; }

        public int ToIndex()
        {
            return Subpieces.subpieces.IndexOf(GetType());
        }

        public SubPiece FromIndex(int index)
        {
            return (SubPiece)Activator.CreateInstance(Subpieces.subpieces[index]);
        }
    }
}