using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zettalith
{
    class SettingsMenu
    {
        MainController controller;

        GameSetup setup;

        GUI.Collection collection, mainCollection, fullscreenCollection, resolutionCollection, volumeCollection;
        Renderer.Text Header, fullscreenText, resolutionText, appliedAtRestartText, masterVolumeText;
        GUI.Button bFullscreen, bResolution, bMasterSpeaker;
        GUI.Button[] bMaster;
        Renderer.SpriteScreen masterSpeaker;

        Action GoBack;

        Texture2D unchecked2D, checked2D, arrow2D, arrowHover2D, arrowPressed2D, uncheckedAudio2D, checkedAudio2D, audioSpeaker2D, mutedAudioSpeaker2D;

        Point[] displays;

        int currentDisplay;

        public void Initialize(MainController controller, Action goBack)
        {
            GoBack = goBack;

            this.controller = controller;

            collection = new GUI.Collection();
            mainCollection = new GUI.Collection() { Origin = Settings.GetHalfResolution };
            fullscreenCollection = new GUI.Collection() { Origin = new Point(0, 0) };
            resolutionCollection = new GUI.Collection() { Origin = new Point(0, 0) };
            volumeCollection = new GUI.Collection() { Origin = new Point(0, 0) };

            bMaster = new GUI.Button[10];

            collection.Add(mainCollection);

            checked2D = Load.Get<Texture2D>("CheckedBox");
            unchecked2D = Load.Get<Texture2D>("UncheckedBox");
            arrow2D = Load.Get<Texture2D>("Arrow");
            arrowHover2D = Load.Get<Texture2D>("ArrowHover");
            arrowPressed2D = Load.Get<Texture2D>("ArrowPressed");
            uncheckedAudio2D = Load.Get<Texture2D>("UncheckedAudioBox");
            checkedAudio2D = Load.Get<Texture2D>("CheckedAudioBox");
            audioSpeaker2D = Load.Get<Texture2D>("AudioSpeaker");
            mutedAudioSpeaker2D = Load.Get<Texture2D>("MutedAudioSpeaker");

            RendererController.GUI.Add(collection);

            Color buttonColor = new Color(220, 220, 220, 255), textColor = new Color(0, 160, 255, 255);

            CustomMasterVolumeCall[] masterCalls = new CustomMasterVolumeCall[10];
            for (int i = 0; i < masterCalls.Length; i++)
            {
                masterCalls[i] = new CustomMasterVolumeCall() { TargetMethod = BMaster, newVolume = (float)(0.1 * (i + 1))};
            }

            //Header = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Styled, "Settings", 10, 0, new Vector2(0, 0));

            #region //Fullscreen
            /*fullscreenText = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, "Fullscreen", 4, 0, new Vector2(0, 0), buttonColor);
            bFullscreen = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle((int)(Settings.GetResolution.X * 0.085), (int)(Settings.GetResolution.Y * 0.0126), (int)(Settings.GetResolution.X * 0.18 / 16), (int)(Settings.GetResolution.Y * 0.02)), PersonalData.Settings.FullScreen ? checked2D : unchecked2D);
            bFullscreen.OnClick += BFullscreen;*/
            #endregion

            #region //Resolution
            /*DisplayModeCollection c = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;
            List<Point> tempResList = new List<Point>();
            float ratio = 16.0f / 9.0f;
            foreach (DisplayMode displayMode in c)
            {
                if (displayMode.AspectRatio == ratio)
                {
                    tempResList.Add(new Point(displayMode.Width, displayMode.Height));
                }
            }
            displays = tempResList.ToArray();

            for (int i = 0; i < displays.Length; ++i)
            {
                if (displays[i] == Settings.GetResolution)
                {
                    currentDisplay = i;
                    i = displays.Length;
                }
            }

            resolutionText = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, "Resolution: " + displays[currentDisplay].Y + "p", 4, 0, new Vector2(0, 0), textColor);
            bResolution = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle((int)(Settings.GetResolution.X * 0.152f), 0, (int)(Settings.GetResolution.X * 0.03f), (int)(Settings.GetResolution.Y * 0.04f)), arrow2D, arrowHover2D, arrowPressed2D) { SpriteEffects = SpriteEffects.FlipHorizontally };
            bResolution.OnClick += BResolution;
            appliedAtRestartText = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, "Settings applied at restart", 4, 0, new Vector2((float)(Settings.GetResolution.X * 0.195f), 0), textColor) { Active = false };*/
            #endregion

            masterVolumeText = new Renderer.Text(new Layer(MainLayer.GUI, 0), Font.Default, "Master Volume", 4, 0, new Vector2((int)(Settings.GetResolution.X * 0.055), 0), textColor);

            for (int i = 0; i < bMaster.Length; i++)
            {
                bMaster[i] = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle((int)(Settings.GetResolution.X * 0.02 * (i + 1)), (int)(Settings.GetResolution.Y * 0.04), (int)(Settings.GetResolution.X * 0.02), (int)(Settings.GetResolution.Y * 0.02 * 112 / 39)), PersonalData.Settings.VolumeMaster >= (0.1 * (i + 1)) ? checkedAudio2D : uncheckedAudio2D);
                bMaster[i].OnClick += masterCalls[i].Activate;
                volumeCollection.Add(bMaster[i]);
            }

            bMasterSpeaker = new GUI.Button(new Layer(MainLayer.GUI, 0), new Rectangle(0, (int)(Settings.GetResolution.Y * 0.04), (int)(Settings.GetResolution.X * 0.02), (int)(Settings.GetResolution.Y * 0.02 * 112 / 39)), PersonalData.Settings.VolumeMaster > 0 ? audioSpeaker2D : mutedAudioSpeaker2D);
            bMasterSpeaker.OnClick += BMasterMute;

            volumeCollection.Add(bMasterSpeaker, masterVolumeText);
            resolutionCollection.Add(resolutionText, bResolution, appliedAtRestartText);
            fullscreenCollection.Add(fullscreenText, bFullscreen);
            mainCollection.Add(Header, fullscreenCollection, resolutionCollection, volumeCollection);
        }

        public void Update()
        {

        }

        private void BMasterMute()
        {
            PersonalData.Settings.VolumeMaster = 0;
            bMasterSpeaker.Texture = mutedAudioSpeaker2D;

            for (int i = 0; i < bMaster.Length; i++)
            {
                bMaster[i].Texture = uncheckedAudio2D;
            }
        }

        private void BMaster(float newVolume)
        {
            PersonalData.Settings.VolumeMaster = newVolume;
            for (int i = 0; i < bMaster.Length; i++)
            {
                if ((i + 1) * 0.1 <= newVolume)
                {
                    bMaster[i].Texture = checkedAudio2D;
                }
                else
                {
                    bMaster[i].Texture = uncheckedAudio2D;
                }
            }
            bMasterSpeaker.Texture = audioSpeaker2D;
        }

        private void BFullscreen()
        {
            if (PersonalData.Settings.FullScreen == true)
            {
                PersonalData.Settings.FullScreen = false;
                bFullscreen.Texture = unchecked2D;
            }
            else
            {
                PersonalData.Settings.FullScreen = true;
                bFullscreen.Texture = checked2D;
            }
            PersonalData.Settings.ApplySettings();
        }

        private void BResolution()
        {
            if (currentDisplay == displays.Length - 1)
                currentDisplay = 0;
            else
                currentDisplay++;

            resolutionText.String = new StringBuilder("Resolution: " + displays[currentDisplay].Y + "p");
            PersonalData.Settings.Resolution = displays[currentDisplay];
            appliedAtRestartText.Active = true;
        }

        private void BGoBack()
        {
            GoBack.Invoke();
        }
    }

    struct CustomMasterVolumeCall
    {
        public Action<float> TargetMethod;
        public float newVolume;

        public void Activate()
        {
            TargetMethod.Invoke(newVolume);
        }
    }
}
