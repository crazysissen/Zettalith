using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zettalith.Pieces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class InGamePiece
    {
        public static InGamePiece[] Pieces { get; private set; } = new InGamePiece[4096];

        public Piece Piece { get; private set; }
        public int Index { get; set; }
        public Texture2D Texture { get; set; }

        Stats baseStats;

        public Top Top { get; private set; } /* => tops[piece.TopIndex]*/
        public Middle Middle { get; private set; } /* => middles[piece.MiddleIndex]*/
        public Bottom Bottom { get; private set; } /* => bottoms[piece.BottomIndex]*/

        public bool IsKing => Top is KingHead;
        public bool Damaged => ModifiedStats.Health < ModifiedStats.MaxHealth;
        public bool HealthBuffed => ModifiedStats.Health > BaseStats.MaxHealth;

        List<Modifier> modifiers = new List<Modifier>();

        public InGamePiece(Piece piece)
        {
            Piece = piece;

            Index = GetNewIndex();
            Pieces[Index] = this;

            //this.piece = piece;
            Top = Subpieces.FromIndex(piece.TopIndex) as Top;
            Middle = Subpieces.FromIndex(piece.MiddleIndex) as Middle;
            Bottom = Subpieces.FromIndex(piece.BottomIndex) as Bottom;

            Texture = ClientSideController.GetTexture(piece.TopIndex, piece.MiddleIndex, piece.BottomIndex);

            baseStats = BaseStats;
        }

        // Returns just the units base stats
        public Stats BaseStats => new Stats()
        {
            AttackDamage = Top.AttackDamage + Middle.AttackDamage + Bottom.AttackDamage,
            MaxHealth = Top.Health + Middle.Health + Bottom.Health,
            Health = Top.Health + Middle.Health + Bottom.Health,
            Mana = Top.ManaCost + Middle.ManaCost + Bottom.ManaCost,
            //AbilityCost = Top.AbilityCost,
            MoveCost = Bottom.MoveCost
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

        // Adds a modifier to this unit
        public void ModThis(Modifier mod)
        {
            modifiers.Add(mod);
        }

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

        public void DestroyIndex()
        {
            Pieces[Index] = null;
        }

        public static int GetNewIndex()
        {
            for (int i = 0; i < Pieces.Length; ++i)
            {
                if (Pieces[i] == null)
                {
                    return i;
                }
            }

            throw new Exception("Piece total exceeded " + Pieces.Length + ", you fucking moron.");
        }
    }
}
