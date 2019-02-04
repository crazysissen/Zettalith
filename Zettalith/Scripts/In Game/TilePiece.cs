using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;

namespace Zettalith
{
    class TilePiece : TileObject
    {
        Piece piece;

        Top Top => piece.Top;
        Middle Middle => piece.Middle;
        Bottom Bottom => piece.Bottom;

        public bool Damaged => ModifiedStats.Health < ModifiedStats.MaxHealth;

        List<Modifier> modifiers = new List<Modifier>();

        public Stats BaseStats => new Stats()
        {
            AttackDamage = Top.AttackDamage + Middle.AttackDamage + Bottom.AttackDamage,
            MaxHealth = Top.Health + Middle.Health + Bottom.Health,
            Health = Top.Health + Middle.Health + Bottom.Health,
            Mana = Top.ManaCost + Middle.ManaCost + Bottom.ManaCost,
            AbilityCost = Top.AbilityCost,
            MoveCost = Bottom.MoveCost
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

        //public TilePiece(Top top, Middle middle, Bottom bottom)
        //{

        //}

        public void Mod(Modifier modifier)
        {
            modifiers.Add(modifier);
        }

        public static void ApplyMod(Modifier mod, TilePiece target)
        {
            target.Mod(mod);
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
