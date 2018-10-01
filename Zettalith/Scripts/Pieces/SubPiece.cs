using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith.Pieces
{
    abstract class SubPiece
    {
        public abstract Mana ManaCost { get; set; }
        public abstract int Health { get; set; }
        public abstract int AttackDamage { get; set; }

    }
}
