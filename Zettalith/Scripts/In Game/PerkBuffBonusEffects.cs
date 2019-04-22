using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    static class PerkBuffBonusEffects
    {
        public static Action<int>[] EffectArray { get; set; }

        static PerkBuffBonusEffects()
        {
            EffectArray = new Action<int>[] 
            {
                //GenericEffect
                BuffHp, BuffAttack, BuffArmor, BuffAbilityDamage, NerfHp, NerfAttack, NerfArmor, NerfAbilityDamage, BuffCost, AbilityCost, HealthIncrease, MovementDecrease, EssenceIncome
            };
        }

        // Skriv metoder för perks/buffs/bonusars effekts här. De får bara ha en int som parameter. Den representerar målet. 0 -> n är indexet i listan av pieces. -1 är jag själv. -2 är motståndaren.

        //public static void GenericEffect(int anInt)
        //{

        //}

        public static void BuffHp(int index)
        {
            Modifier mod = new Addition(new Stats(0, 1, 0, new Mana(), new Mana(), new Mana()), true);
            (InGameController.Grid[index] as TilePiece).Piece.ModThis(mod);
        }

        public static void BuffAttack(int index)
        {
            Modifier mod = new Addition(new Stats(1, 0, 0, new Mana(), new Mana(), new Mana()), true);
            (InGameController.Grid[index] as TilePiece).Piece.ModThis(mod);
        }

        public static void BuffArmor(int index)
        {
            Modifier mod = new Addition(new Stats(1, true), true);
            (InGameController.Grid[index] as TilePiece).Piece.ModThis(mod);
        }

        public static void BuffAbilityDamage(int index)
        {
            InGamePiece piece = (InGameController.Grid[index] as TilePiece).Piece;

            if (piece.Top.Modifier is Addition && piece.Top.Modifier.StatChanges.Health < 0)
            {
                int healthChange = piece.Top.Modifier.StatChanges.Health;
                piece.Top.Modifier = new Addition(new Stats(healthChange - 1), true);
            }
        }

        public static void NerfHp(int index)
        {
            Modifier mod = new Addition(new Stats(0, -1, 0, new Mana(), new Mana(), new Mana()), true);
            (InGameController.Grid[index] as TilePiece).Piece.ModThis(mod);
        }

        public static void NerfAttack(int index)
        {
            Modifier mod = new Addition(new Stats(-1, 0, 0, new Mana(), new Mana(), new Mana()), true);
            (InGameController.Grid[index] as TilePiece).Piece.ModThis(mod);
        }

        public static void NerfArmor(int index)
        {
            Modifier mod = new Addition(new Stats(-1, true), true);
            (InGameController.Grid[index] as TilePiece).Piece.ModThis(mod);
        }

        public static void NerfAbilityDamage(int index)
        {
            InGamePiece piece = (InGameController.Grid[index] as TilePiece).Piece;

            if (piece.Top.Modifier is Addition && piece.Top.Modifier.StatChanges.Health < 0)
            {
                int healthChange = piece.Top.Modifier.StatChanges.Health;
                piece.Top.Modifier = new Addition(new Stats(healthChange + 1), true);
            }
        }

        public static void BuffCost(int index)
        {
            Ztuff.BuffCostFactor = 0.7f;
            Ztuff.changeBuffCost = true;
        }

        public static void AbilityCost(int index)
        {
            // Planen
            for (int i = 0; i < InGameController.Grid.Objects.Length; ++i)
            {
                if ((InGameController.Grid.Objects[i] as TilePiece).Player == InGameController.PlayerIndex)
                {
                    (InGameController.Grid.Objects[i] as TilePiece).Piece.Top.AbilityCost = new Mana((int)((InGameController.Grid.Objects[i] as TilePiece).Piece.Top.AbilityCost.Red * Ztuff.abilityCostFactor), (int)((InGameController.Grid.Objects[i] as TilePiece).Piece.Top.AbilityCost.Green * Ztuff.abilityCostFactor), (int)((InGameController.Grid.Objects[i] as TilePiece).Piece.Top.AbilityCost.Blue * Ztuff.abilityCostFactor));
                }
            }
            // Decket
            for (int i = 0; i < InGameController.Local.Deck.Pieces.Count; ++i)
            {
                InGameController.Local.Deck.Pieces[i].Top.AbilityCost = new Mana((int)(InGameController.Local.Deck.Pieces[i].Top.AbilityCost.Red * Ztuff.abilityCostFactor), (int)(InGameController.Local.Deck.Pieces[i].Top.AbilityCost.Green * Ztuff.abilityCostFactor), (int)(InGameController.Local.Deck.Pieces[i].Top.AbilityCost.Blue * Ztuff.abilityCostFactor));
            }
            // Handen
            for (int i = 0; i < InGameController.Local.Hand.Count; ++i)
            {
                InGameController.Local.Hand[i].Top.AbilityCost = new Mana((int)(InGameController.Local.Hand[i].Top.AbilityCost.Red * Ztuff.abilityCostFactor), (int)(InGameController.Local.Hand[i].Top.AbilityCost.Green * Ztuff.abilityCostFactor), (int)(InGameController.Local.Hand[i].Top.AbilityCost.Blue * Ztuff.abilityCostFactor));
            }
        }

        public static void HealthIncrease(int index)
        {
            // Decket
            for (int i = 0; i < InGameController.Local.Deck.Pieces.Count; ++i)
            {
                InGameController.Local.Deck.Pieces[i].Top.Health = (int)(InGameController.Local.Deck.Pieces[i].Top.Health * 1.5f);
                InGameController.Local.Deck.Pieces[i].Middle.Health = (int)(InGameController.Local.Deck.Pieces[i].Middle.Health * 1.5f);
                InGameController.Local.Deck.Pieces[i].Bottom.Health = (int)(InGameController.Local.Deck.Pieces[i].Bottom.Health * 1.5f);
            }
            // Handen
            for (int i = 0; i < InGameController.Local.Hand.Count; ++i)
            {
                InGameController.Local.Hand[i].Top.Health = (int)(InGameController.Local.Hand[i].Top.Health * 1.5f);
                InGameController.Local.Hand[i].Middle.Health = (int)(InGameController.Local.Hand[i].Middle.Health * 1.5f);
                InGameController.Local.Hand[i].Bottom.Health = (int)(InGameController.Local.Hand[i].Bottom.Health * 1.5f);
            }
        }

        public static void MovementDecrease(int index)
        {
            // Decket
            for (int i = 0; i < InGameController.Local.Deck.Pieces.Count; ++i)
            {
                InGameController.Local.Deck.Pieces[i].Bottom.MovementTime = InGameController.Local.Deck.Pieces[i].Bottom.MovementTime * (1 / 1.3f);
            }
            // Handen
            for (int i = 0; i < InGameController.Local.Hand.Count; ++i)
            {
                InGameController.Local.Hand[i].Bottom.MovementTime = InGameController.Local.Hand[i].Bottom.MovementTime = (1 / 1.3f);
            }
        }

        public static void EssenceIncome(int index)
        {

        }
    }

    [Serializable]
    class EffectCache
    {
        public List<Sints> AListOfSints;

        public EffectCache()
        {
            AListOfSints = new List<Sints>();
        }
    }

    [Serializable]
    struct Sints
    {
        public int IntA, IntB;

        public Sints(int intOne, int intTwo)
        {
            IntA = intOne;
            IntB = intTwo;
        }
    }
}
