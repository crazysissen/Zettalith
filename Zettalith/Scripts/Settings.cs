using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    [Serializable]
    static class Settings
    {
        public static Point Resolution => new Point(XNAController.Graphics.PreferredBackBufferWidth, XNAController.Graphics.PreferredBackBufferHeight);
        public static Point HalfResolution => new Point(Resolution.X / 2, Resolution.Y / 2);

        public static void Intitialize()
        {

        }
    }
}
