using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Zettalith
{
    [Serializable]
    class Settings
    {
        public static Point GetResolution => new Point(XNAController.Graphics.PreferredBackBufferWidth, XNAController.Graphics.PreferredBackBufferHeight);
        public static Point GetHalfResolution => new Point(GetResolution.X / 2, GetResolution.Y / 2);

        public Point Resolution { get => resolution; set => resolution = value; }
        public bool FullScreen { get; set; }

        public float VolumeMaster { get; set; } = 0.8f;
        public float VolumeMusic { get { return VolumeMusic * VolumeMaster; } set { volumeMusic = value; } }
        public float VolumeSFX { get { return VolumeSFX * VolumeMaster; } set { volumeSFX = value; } }
        public float VolumeAmbient { get { return VolumeAmbient * VolumeMaster; } set { volumeAmbient = value; } }

        float
            volumeMusic = 1,
            volumeSFX = 1,
            volumeAmbient = 1;

        SPoint
            resolution = GetResolution;

        public void ApplySettings()
        {
            // TODO: Apply all settings
            XNAController.Graphics.PreferredBackBufferWidth = Resolution.X;
            XNAController.Graphics.PreferredBackBufferHeight = Resolution.Y;
            XNAController.Graphics.IsFullScreen = PersonalData.Settings.FullScreen;

            XNAController.Graphics.ApplyChanges();
        }

        public void Intitialize()
        {

        }
    }

    [Serializable]
    class SPoint
    {
        public int X, Y;

        public SPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator SPoint(Point point) => new SPoint(point.X, point.Y);
        public static implicit operator Point(SPoint point) => new Point(point.X, point.Y);
    }
}
