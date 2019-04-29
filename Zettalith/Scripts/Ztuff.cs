using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    static class Ztuff
    {
        public static float
            essenceFactor = 1f,
            buffCostFactor = 1f,
            SizeResFactor;
        public static int
            healthIncrease,
            placeHeightIncrease,
            incomingEffect,
            essenceCost;
        public static bool pickingPiece = false;
        static GUI.Collection theGUI;
        public static bool changeBuffCost = false;
        public static Mana 
            abilityCostDecrease = new Mana(),
            buffCost = new Mana();

        //public static int[] healthIncrease = new int[2] { 0, 0 };

        static Ztuff()
        {
            SizeResFactor = Settings.GetResolution.Y / 1080f;
        }

        public static void RecieveGUI(GUI.Collection gui)
        {
            theGUI = gui;
        }

        public static void RestoreFromBuff()
        {
            if (theGUI != null)
            {
                theGUI.Active = true;
            }
        }
    }
}
