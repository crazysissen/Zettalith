using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith.Pieces
{
    class Piece
    {
        public Stats BaseStats => new Stats()
        {
            Health = top.Health + middle.Health + bottom.Health,
            Mana = top.ManaCost + middle.ManaCost + bottom.ManaCost,
            AbilityCost = top.AbilityCost,
            MoveCost = bottom.MoveCost
        };

        public Modifier CumulativeModifier
        {
            get
            {
                Modifier cumulative = new Modifier();

                foreach (Modifier modifier in modifiers)
                {
                    cumulative.StatChanges += modifier.StatChanges;

                    foreach (string mod in modifier.DirectModifiers)
                    {
                        if (!cumulative.DirectModifiers.Contains(mod))
                            cumulative.DirectModifiers.Add(mod);
                    }
                }

                return cumulative;
            }
        }

        public Stats ModifiedStats => BaseStats + CumulativeModifier.StatChanges;

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