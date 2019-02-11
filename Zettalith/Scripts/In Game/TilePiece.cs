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
        Stats baseStats;

        Piece piece;

        Top top/* => tops[piece.TopIndex]*/;
        Middle middle/* => middles[piece.MiddleIndex]*/;
        Bottom bottom/* => bottoms[piece.BottomIndex]*/;

        public bool Damaged => ModifiedStats.Health < ModifiedStats.MaxHealth;
        public bool HealthBuffed => ModifiedStats.Health > BaseStats.MaxHealth;

        public int Index { get; set; }
        
        List<Modifier> modifiers = new List<Modifier>();

        public TilePiece(Piece piece, int index)
        {
            //this.piece = piece;
            top = Subpieces.FromIndex(piece.TopIndex) as Top;
            middle = Subpieces.FromIndex(piece.MiddleIndex) as Middle;
            bottom = Subpieces.FromIndex(piece.BottomIndex) as Bottom;
            baseStats = BaseStats;
            Index = index;
        }

        // Returns just the units base stats
        public Stats BaseStats => new Stats()
        {
            AttackDamage = top.AttackDamage + middle.AttackDamage + bottom.AttackDamage,
            MaxHealth = top.Health + middle.Health + bottom.Health,
            Health = top.Health + middle.Health + bottom.Health,
            Mana = top.ManaCost + middle.ManaCost + bottom.ManaCost,
            //AbilityCost = Top.AbilityCost,
            MoveCost = bottom.MoveCost
        };

        // Returns a units base stats with all modifications applied
        public Stats ModifiedStats
        {
            get
            {
                Stats modified = baseStats;

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
                    else if (modifier is Direct)
                    {
                        ClearMods();
                        modified = (modifier as Direct).StatChanges; 
                    }
                }

                return modified;
            }
        }

        // TODO: GameAction?
        // Adds a modifier to this unit
        public void ModThis(Modifier mod)
        {
            modifiers.Add(mod);
        }

        // TODO: GameAction?
        // Clears modifications that are not permanent
        public void ClearMods()
        {
            List<Modifier> remove = new List<Modifier>();

            foreach (Modifier modifier in modifiers)
            {
                if (!modifier.Permanent || modifier is Direct)
                {
                    remove.Add(modifier);
                }
            }

            remove.Clear();
        }

        // Clears all mods, aka resets the unit to its factory state
        public void ResetMods()
        {
            modifiers.Clear();
        }
    }
}
