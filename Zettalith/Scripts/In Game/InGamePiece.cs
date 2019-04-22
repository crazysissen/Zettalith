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

        public Mana GetCost => ModifiedStats.Mana;

        public Piece Piece { get; private set; }
        public int Index { get; set; }
        public Texture2D Texture { get; set; }

        //Stats baseStats;

        public Top Top { get; private set; } /* => tops[piece.TopIndex]*/
        public Middle Middle { get; private set; } /* => middles[piece.MiddleIndex]*/
        public Bottom Bottom { get; private set; } /* => bottoms[piece.BottomIndex]*/

        public bool IsKing => Top is KingHead && Middle is KingMiddle && Bottom is KingFeet;
        public bool Damaged => ModifiedStats.Health < ModifiedStats.MaxHealth;
        public bool HealthBuffed => ModifiedStats.Health > BaseStats.MaxHealth;

        public bool HasAttacked { get; set; }
        public bool HasMoved { get; set; }

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

            //baseStats = BaseStats;
        }

        // Returns just the units base stats
        public Stats BaseStats => new Stats()
        {
            AttackDamage = Top.AttackDamage + Middle.AttackDamage + Bottom.AttackDamage,
            MaxHealth = Top.Health + Middle.Health + Bottom.Health + Ztuff.healthIncrease[(InGameController.Grid[Index] as TilePiece).Player],
            Health = Top.Health + Middle.Health + Bottom.Health + Ztuff.healthIncrease[(InGameController.Grid[Index] as TilePiece).Player],
            Armor = Top.Armor + Middle.Armor + Bottom.Armor,
            Mana = Top.ManaCost + Middle.ManaCost + Bottom.ManaCost,
            AbilityCost = Top.AbilityCost - Ztuff.abilityCostDecrease,
            MoveCost = Bottom.MoveCost
        };

        // Returns a units base stats with all modifications applied
        public Stats ModifiedStats
        {
            get
            {
                Stats modified = BaseStats;

                foreach (Modifier modifier in modifiers)
                {
                    if (modifier is Addition)
                    {
                        Modifier mod = new Addition(modifier.StatChanges, true);

                        if (modified.Armor > 0)
                        {
                            modified.Armor += mod.StatChanges.Health;

                            if (modified.Armor < 0)
                            {
                                mod.StatChanges = new Stats(mod.StatChanges.AttackDamage, modified.Armor, 0, mod.StatChanges.Mana, mod.StatChanges.AbilityCost, mod.StatChanges.MoveCost);
                                modified += mod.StatChanges;
                                modified.Armor = 0;
                            }
                            else
                            {
                                modified += new Stats(mod.StatChanges.AttackDamage, 0, 0, mod.StatChanges.Mana, mod.StatChanges.AbilityCost, mod.StatChanges.MoveCost);
                            }
                        }
                        else
                        {
                            modified += mod.StatChanges;
                        }
                        // modified += mod.StatChanges;
                    }
                    else if (modifier is Multiplication)
                    {
                        Modifier mod = new Multiplication(modifier.StatChanges, true);

                        modified *= mod.StatChanges;
                    }
                    else if (modifier is Direct)
                    {
                        Modifier mod = new Direct(modifier.StatChanges, true);

                        if (!(mod.StatChanges.AbilityCost == new Mana()))
                        {
                            modified.AbilityCost = mod.StatChanges.AbilityCost;
                        }
                        if (mod.StatChanges.Health > 0)
                        {
                            modified.Health = mod.StatChanges.Health;
                        }
                        if (mod.StatChanges.Armor >= 0)
                        {
                            modified.Armor = mod.StatChanges.Armor;
                        }
                        if (!(mod.StatChanges.Mana == new Mana()))
                        {
                            modified.Mana = mod.StatChanges.Mana;
                        }
                        if (!(mod.StatChanges.MoveCost == new Mana()))
                        {
                            modified.MoveCost = mod.StatChanges.MoveCost;
                        }
                        if (mod.StatChanges.AttackDamage > 0)
                        {
                            modified.AttackDamage = mod.StatChanges.AttackDamage;
                        }

                        //ClearMods();
                        //modified = (modifier as Direct).StatChanges;
                    }
                }

                if (modified.Health > modified.MaxHealth)
                {
                    modified.Health = modified.MaxHealth;
                }

                return modified;
            }
        }

        // Adds a modifier to this unit
        public void ModThis(Modifier mod)
        {
            modifiers.Add(mod);
        }

        // Clears modifications that are not permanent, aka debuffs
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

            for (int i = 0; i < remove.Count; ++i)
            {
                modifiers.Remove(remove[i]);
                i--;
            }
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
