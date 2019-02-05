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

        Top top/* => tops[piece.TopIndex]*/;
        Middle middle/* => middles[piece.MiddleIndex]*/;
        Bottom bottom/* => bottoms[piece.BottomIndex]*/;

        public bool Damaged => ModifiedStats.Health < ModifiedStats.MaxHealth;

        List<Modifier> modifiers = new List<Modifier>();

        public TilePiece(Piece piece)
        {
            this.piece = piece;
            top = top.FromIndex(piece.TopIndex);
            middle = middle.FromIndex(piece.MiddleIndex);
            bottom = bottom.FromIndex(piece.BottomIndex);
        }

        public Stats BaseStats => new Stats()
        {
            AttackDamage = top.AttackDamage + middle.AttackDamage + bottom.AttackDamage,
            MaxHealth = top.Health + middle.Health + bottom.Health,
            Health = top.Health + middle.Health + bottom.Health,
            Mana = top.ManaCost + middle.ManaCost + bottom.ManaCost,
            //AbilityCost = Top.AbilityCost,
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
