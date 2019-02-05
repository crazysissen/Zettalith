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
        public Ability Ability { get; set; }
    }
}