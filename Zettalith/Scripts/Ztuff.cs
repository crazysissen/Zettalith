﻿using System;
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
        public static float SizeResFactor;
        public static int incomingEffect;
        public static bool pickingPiece = false;
        static GUI.Collection theGUI;

        static Ztuff()
        {
            SizeResFactor = Settings.GetResolution.Y / 1080f;
        }

        public static void RecieveEffect(int effect, GUI.Collection gui)
        {
            incomingEffect = effect;
            theGUI = gui;
        }

        public static void RestoreFromBuff()
        {
            theGUI.Active = true;
        }
    }
}