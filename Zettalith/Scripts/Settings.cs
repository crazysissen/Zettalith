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
        public float VolumeMusicForMenu { get; set; } = 0.8f;
        public float VolumeSFXForMenu { get; set; } = 0.8f;
        public float VolumeAmbientForMenu { get; set; } = 0.8f;

        public float VolumeMusic { get { return VolumeMusicForMenu * VolumeMaster; } set { VolumeMusicForMenu = value; } }
        public float VolumeSFX { get { return VolumeSFXForMenu * VolumeMaster; } set { VolumeSFXForMenu = value; } }
        public float VolumeAmbient { get { return VolumeAmbientForMenu * VolumeMaster; } set { VolumeAmbientForMenu = value; } }

        SPoint
            resolution = GetResolution;

        public void ApplySettings()
        {
            // TODO: Apply all settings
            // Tog bort ändring av XNAController.Graphics.PreferredBackBuffer härifrån. Det ska bara hända vid startup.
            XNAController.Graphics.IsFullScreen = PersonalData.Settings.FullScreen;

            XNAController.Graphics.ApplyChanges();
        }

        public void Intitialize()
        {
            XNAController.Graphics.PreferredBackBufferWidth = PersonalData.Settings.Resolution.X;
            XNAController.Graphics.PreferredBackBufferHeight = PersonalData.Settings.Resolution.Y;
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
