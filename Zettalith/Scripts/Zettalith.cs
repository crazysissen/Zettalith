using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    class Zettalith
    {
        public Mana ManaCostModifier { get; private set; }

        public Mana ManaCost => top.ManaCost + middle.ManaCost + bottom.ManaCost + ManaCostModifier;
        public Mana AbilityCost => top.AbilityCost;
        public Mana MoveCost => bottom.MoveCost;

        Top top;
        Middle middle;
        Bottom bottom;

        public Zettalith(Top top, Middle middle, Bottom bottom)
        {
            this.top = top;
            this.middle = middle;
            this.bottom = bottom;
        }

        public void ModifyMana(Mana modifier)
        {
            ManaCostModifier += modifier;
        }
    }
}