﻿using System;
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
                GenericEffect

            };
        }

        // Skriv metoder för perks/buffs/bonusars effekts här. De får bara ha en int som parameter. Den representerar målet. 0 -> n är indexet i listan av pieces. -1 är jag själv. -2 är motståndaren.

        public static void GenericEffect(int anInt)
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