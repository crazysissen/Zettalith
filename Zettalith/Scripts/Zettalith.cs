using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    class Piece
    {
        public int Health => top.Health + middle.Health + bottom.Health;
        public int AttackDamage => top.AttackDamage + middle.AttackDamage + bottom.AttackDamage;

        public Mana ManaCost => top.ManaCost + middle.ManaCost + bottom.ManaCost;
        public Mana AbilityCost => top.AbilityCost;
        public Mana MoveCost => bottom.MoveCost;

        List<Modifier> modifiers = new List<Modifier>();

        Top top;
        Middle middle;
        Bottom bottom;

        public Piece(Top top, Middle middle, Bottom bottom)
        {
            this.top = top;
            this.middle = middle;
            this.bottom = bottom;
        }
    }
}