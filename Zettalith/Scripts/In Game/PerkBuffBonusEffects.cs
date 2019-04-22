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
                BuffHp, BuffAttack, BuffArmor, BuffAbilityDamage, NerfHp, NerfAttack, NerfArmor, NerfAbilityDamage, BuffCost, AbilityCost, MoveCost, KingMove, EssenceIncome
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
            if (index + 2 == InGameController.PlayerIndex)
            {
                Ztuff.BuffCostFactor = 0.7f;
                Ztuff.changeBuffCost = true;
            }
        }

        public static void AbilityCost(int index)
        {
            if (index + 2 == InGameController.PlayerIndex)
            {
                Ztuff.abilityCostDecrease = new Mana(1, 1, 1);
            }
        }

        public static void MoveCost(int index)
        {
            //Ztuff.healthIncrease[InGameController.PlayerIndex] = 5;
        }

        public static void KingMove(int index)
        {
            if (index + 2 == InGameController.PlayerIndex)
            {
                InGameController.Local.King.Piece.Bottom.MoveRange = 2;
            }
        }

        public static void EssenceIncome(int index)
        {
            if (index + 2 == InGameController.PlayerIndex)
            {
                Ztuff.essenceFactor *= 0.7f;
            }
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
