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
        public abstract string Name { get; }
        public abstract Mana ManaCost { get; }
        public abstract int Health { get; }
        public abstract int AttackDamage { get; }

        public abstract Ability Ability { get; }
    }
}
