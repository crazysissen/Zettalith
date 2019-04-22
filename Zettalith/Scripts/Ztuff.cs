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
            abilityCostFactor = 0.7f,
            SizeResFactor;
        public static int incomingEffect;
        public static bool pickingPiece = false;
        static GUI.Collection theGUI;
        public static float BuffCostFactor = 1;
        public static bool changeBuffCost = false;

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
