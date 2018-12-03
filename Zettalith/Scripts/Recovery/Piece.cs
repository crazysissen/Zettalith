using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
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

        public Stats ModifiedStats
        {
            get
            {
                Stats modified = BaseStats;

                foreach (Modifier modifier in modifiers)
                {
                    if (modifier is Addition)
                    {
                        modified += (modifier as Addition).StatChanges;
                    }
                    else if (modifier is Multiplication)
                    {
                        modified *= (modifier as Multiplication).StatChanges;
                    }
                    //else if (modifier is NEWMODIFIER)
                    //{

                    //}
                }

                return modified;
            }
        }

        public bool Damaged { get; private set; }

        List<Modifier> modifiers = new List<Modifier>();
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

        Top top;
        Middle middle;
        Bottom bottom;

        public Piece(Top top, Middle middle, Bottom bottom)
        {
            this.top = top;
            this.middle = middle;
            this.bottom = bottom;
        }

        public void Mod(Modifier modifier)
        {
            modifiers.Add(modifier);
        }

        public void ApplyMod(Modifier mod, Piece target = null, List<Piece> targets = null)
        {
            if (target != null)
            {
                target.Mod(mod);
            }
            if (targets != null)
            {
                foreach (Piece piece in targets)
                {
                    piece.Mod(mod);
                }
            }
        }

        public void ClearMods()
        {
            foreach (Modifier modifier in modifiers)
            {
                if (!modifier.Permanent)
                {
                    modifiers.Remove(modifier);
                }
            }
        }

        public void ResetMods()
        {
            modifiers.Clear();
        }
    }
}