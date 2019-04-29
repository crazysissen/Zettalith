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

        GUI.Collection collection, mainCollection, fullscreenCollection, resolutionCollection, volumeCollection, masterVolCollection, musicVolCollection, sFXVolCollection, ambientVolCollection;
        Renderer.Text header, fullscreenText, resolutionText, appliedAtRestartText, masterVolumeText, musicVolumeText, sFXVolumeText, ambientVolumeText;
        GUI.Button bFullscreen, bResolution, bMasterSpeaker, bMusicSpeaker, bSFXSpeaker, bAmbientSpeaker, bBack;
        GUI.Button[] bMaster, bMusic, bSFX, bAmbient;

        Action GoBack;

        Texture2D unchecked2D, checked2D, arrow2D, arrowHover2D, arrowPressed2D, uncheckedAudio2D, checkedAudio2D, audioSpeaker2D, mutedAudioSpeaker2D;

        Point[] displays;

        int currentDisplay, masterToApply, musicToApply, sFXToApply, ambientToApply;
        float defaultFontSize;

        Color buttonColor = new Color(220, 220, 220, 255), textColor = Color.White;

        CustomVolumeCall[] masterVolumeCalls = new CustomVolumeCall[10];
        CustomVolumeCall[] musicVolumeCalls = new CustomVolumeCall[10];
        CustomVolumeCall[] sFXVolumeCalls = new CustomVolumeCall[10];
        CustomVolumeCall[] ambientVolumeCalls = new CustomVolumeCall[10];

        RendererFocus focusSettings;

        Layer settingsLayer;

        public void Initialize(MainController controller, Action goBack, Layer useThisLayer)
        {
            settingsLayer = useThisLayer;

            GoBack = goBack;

            this.controller = controller;

            focusSettings = new RendererFocus(settingsLayer);

            collection = new GUI.Collection();
            mainCollection = new GUI.Collection() { Origin = new Point((int)(Settings.GetHalfResolution.X - Settings.GetResolution.X * 0.143), (int)(Settings.GetResolution.Y * 0.18)) };
            fullscreenCollection = new GUI.Collection() { Origin = new Point((int)(Settings.GetResolution.X * 0.075), (int)(Settings.GetResolution.Y * 0.15)) };
            resolutionCollection = new GUI.Collection() { Origin = new Point((int)(Settings.GetResolution.X * 0.045), (int)(Settings.GetResolution.Y * 0.2)) };
            volumeCollection = new GUI.Collection() { Origin = new Point((int)(Settings.GetResolution.X * 0.015), (int)(Settings.GetResolution.Y * 0.25)) };
            masterVolCollection = new GUI.Collection() { Origin = new Point(0, 0) };
            musicVolCollection = new GUI.Collection() { Origin = new Point(0, (int)(Settings.GetResolution.Y * 0.1)) };
            sFXVolCollection = new GUI.Collection() { Origin = new Point(0, (int)(Settings.GetResolution.Y * 0.2)) };
            ambientVolCollection = new GUI.Collection() { Origin = new Point(0, (int)(Settings.GetResolution.Y * 0.3)) };

            volumeCollection.Add(masterVolCollection, musicVolCollection, sFXVolCollection, ambientVolCollection);

            defaultFontSize = 4 * (Settings.GetResolution.Y / 1080f);

            bMaster = new GUI.Button[10];
            bMusic = new GUI.Button[10];
            bSFX = new GUI.Button[10];
            bAmbient = new GUI.Button[10];

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

            for (int i = 0; i < masterVolumeCalls.Length; i++) { masterVolumeCalls[i] = new CustomVolumeCall() { TargetMethod = CallBMasterAudioChange, newVolume = i + 1,  }; }
            for (int i = 0; i < musicVolumeCalls.Length; i++) { musicVolumeCalls[i] = new CustomVolumeCall() { TargetMethod = CallBMusicAudioChange, newVolume = i + 1, }; }
            for (int i = 0; i < sFXVolumeCalls.Length; i++) { sFXVolumeCalls[i] = new CustomVolumeCall() { TargetMethod = CallBSFXAudioChange, newVolume = i + 1, }; }
            for (int i = 0; i < ambientVolumeCalls.Length; i++) { ambientVolumeCalls[i] = new CustomVolumeCall() { TargetMethod = CallBAmbientAudioChange, newVolume = i + 1, }; }

            Action CallMasterMute = CallBMasterAudioMute;
            Action CallMusicMute = CallBMusicAudioMute;
            Action CallSFXMute = CallBSFXAudioMute;
            Action CallAmbientMute = CallBAmbientAudioMute;
            
            masterToApply = (int)(PersonalData.Settings.VolumeMaster * 10);
            musicToApply = (int)(PersonalData.Settings.VolumeMusicForMenu * 10);
            sFXToApply = (int)(PersonalData.Settings.VolumeSFXForMenu * 10);
            ambientToApply = (int)(PersonalData.Settings.VolumeAmbientForMenu * 10);

            header = new Renderer.Text(settingsLayer, Font.Styled, "Settings", 10f * (Settings.GetResolution.Y / 1080f), 0, new Vector2(0, 0));

            #region //Fullscreen
            fullscreenText = new Renderer.Text(settingsLayer, Font.Default, "Fullscreen", defaultFontSize, 0, new Vector2(0, 0), buttonColor);
            bFullscreen = new GUI.Button(settingsLayer, new Rectangle((int)(Settings.GetResolution.X * 0.085), (int)(Settings.GetResolution.Y * 0.0126), (int)(Settings.GetResolution.X * 0.18 / 16), (int)(Settings.GetResolution.Y * 0.02)), PersonalData.Settings.FullScreen ? checked2D : unchecked2D);
            bFullscreen.OnClick += BFullscreen;
            #endregion

            #region //Resolution
            DisplayModeCollection c = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;
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
                if (displays[i] == PersonalData.Settings.Resolution)
                {
                    currentDisplay = i;
                    i = displays.Length;
                }
            }

            resolutionText = new Renderer.Text(settingsLayer, Font.Default, "Resolution: " + displays[currentDisplay].Y + "p", defaultFontSize, 0, new Vector2(0, 0), textColor);
            bResolution = new GUI.Button(settingsLayer, new Rectangle((int)(Settings.GetResolution.X * 0.152f), 0, (int)(Settings.GetResolution.X * 0.03f), (int)(Settings.GetResolution.Y * 0.04f)), arrow2D, arrowHover2D, arrowPressed2D) { SpriteEffects = SpriteEffects.FlipHorizontally };
            bResolution.OnClick += BResolution;
            appliedAtRestartText = new Renderer.Text(settingsLayer, Font.Default, "Settings applied at restart", defaultFontSize, 0, new Vector2((float)(Settings.GetResolution.X * 0.195f), 0), textColor) { Active = false };
            #endregion

            #region //Volume
            CreateAudioBar(ref masterVolumeText, "Master Volume", ref bMaster, masterToApply, ref masterVolCollection, ref bMasterSpeaker, masterVolumeCalls, CallBMasterAudioMute);
            CreateAudioBar(ref musicVolumeText, "Music Volume", ref bMusic, musicToApply, ref musicVolCollection, ref bMusicSpeaker, musicVolumeCalls, CallBMusicAudioMute);
            CreateAudioBar(ref sFXVolumeText, "SFX Volume", ref bSFX, sFXToApply, ref sFXVolCollection, ref bSFXSpeaker, sFXVolumeCalls, CallBSFXAudioMute);
            CreateAudioBar(ref ambientVolumeText, "Ambient Volume", ref bAmbient, ambientToApply, ref ambientVolCollection, ref bAmbientSpeaker, ambientVolumeCalls, CallBAmbientAudioMute);
            #endregion

            Texture2D backTexture = Load.Get<Texture2D>("Button1");
            bBack = new GUI.Button(settingsLayer, new Rectangle((int)(Settings.GetResolution.X * 0.1), (int)(Settings.GetResolution.Y * 0.68f), (int)(Ztuff.SizeResFactor * backTexture.Width * 2), (int)(Ztuff.SizeResFactor * backTexture.Height * 2)), backTexture) { ScaleEffect = true };
            bBack.AddText("Back", defaultFontSize, true, textColor, Font.Default);
            bBack.OnClick += BGoBack;

            masterVolCollection.Add(bMasterSpeaker, masterVolumeText);
            musicVolCollection.Add(bMusicSpeaker, musicVolumeText);
            sFXVolCollection.Add(bSFXSpeaker, sFXVolumeText);
            ambientVolCollection.Add(bAmbientSpeaker, ambientVolumeText);
            resolutionCollection.Add(resolutionText, bResolution, appliedAtRestartText);
            fullscreenCollection.Add(fullscreenText, bFullscreen);
            mainCollection.Add(header, fullscreenCollection, resolutionCollection, volumeCollection, bBack);
        }

        public void Update()
        {

        }

        private void CallBMasterAudioMute() { BAudioMute(ref masterToApply, ref bMasterSpeaker, ref bMaster); }
        private void CallBMusicAudioMute() { BAudioMute(ref musicToApply, ref bMusicSpeaker, ref bMusic); }
        private void CallBSFXAudioMute() { BAudioMute(ref sFXToApply, ref bSFXSpeaker, ref bSFX); }
        private void CallBAmbientAudioMute() { BAudioMute(ref ambientToApply, ref bAmbientSpeaker, ref bAmbient); }

        private void BAudioMute(ref int currentVolume, ref GUI.Button bSpeaker, ref GUI.Button[] buttonArray)
        {
            currentVolume = 0;
            bSpeaker.Texture = mutedAudioSpeaker2D;

            for (int i = 0; i < buttonArray.Length; i++)
            {
                buttonArray[i].Texture = uncheckedAudio2D;
            }

            ApplyVolumes();
        }

        private void CallBMasterAudioChange(int newVolume) { BAudioChange(newVolume, ref masterToApply, ref bMaster, ref bMasterSpeaker); }
        private void CallBMusicAudioChange(int newVolume) { BAudioChange(newVolume, ref musicToApply, ref bMusic, ref bMusicSpeaker); }
        private void CallBSFXAudioChange(int newVolume) { BAudioChange(newVolume, ref sFXToApply, ref bSFX, ref bSFXSpeaker); }
        private void CallBAmbientAudioChange(int newVolume) { BAudioChange(newVolume, ref ambientToApply, ref bAmbient, ref bAmbientSpeaker); }

        private void BAudioChange(int newVolume, ref int currentVolume, ref GUI.Button[] buttonArray, ref GUI.Button bSpeaker)
        {
            currentVolume = newVolume;

            for (int i = 0; i < buttonArray.Length; i++)
            {
                if (i + 1 <= newVolume)
                {
                    buttonArray[i].Texture = checkedAudio2D;
                }
                else
                {
                    buttonArray[i].Texture = uncheckedAudio2D;
                }
            }
            bSpeaker.Texture = audioSpeaker2D;

            ApplyVolumes();
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
            SaveLoad.Save();
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
            SaveLoad.Save();
        }

        private void BGoBack()
        {
            SaveLoad.Save();

            Close();
        }

        public void Close()
        {
            focusSettings.Remove();

            collection.Active = false;
            GoBack.Invoke();
        }

        private void CreateAudioBar(ref Renderer.Text textVar, string printedText, ref GUI.Button[] buttonArray, float currentVolume, ref GUI.Collection relevantCollection, ref GUI.Button bSpeaker, CustomVolumeCall[] calls, Action muteMethod)
        {
            textVar = new Renderer.Text(settingsLayer, Font.Default, printedText, defaultFontSize, 0, new Vector2((int)(Settings.GetResolution.X * 0.055), 0), textColor);

            for (int i = 0; i < buttonArray.Length; i++)
            {
                buttonArray[i] = new GUI.Button(settingsLayer, new Rectangle((int)(Settings.GetResolution.X * 0.02 * (i + 1)), (int)(Settings.GetResolution.Y * 0.04), (int)(Settings.GetResolution.X * 0.02), (int)(Settings.GetResolution.Y * 0.02 * 112 / 39)), currentVolume >= i + 1 ? checkedAudio2D : uncheckedAudio2D);
                buttonArray[i].OnClick += calls[i].Activate;
                relevantCollection.Add(buttonArray[i]);
            }

            bSpeaker = new GUI.Button(settingsLayer, new Rectangle(0, (int)(Settings.GetResolution.Y * 0.04), (int)(Settings.GetResolution.X * 0.02), (int)(Settings.GetResolution.Y * 0.02 * 112 / 39)), currentVolume > 0 ? audioSpeaker2D : mutedAudioSpeaker2D);
            bSpeaker.OnClick += muteMethod;
        }

        private void ApplyVolumes()
        {
            PersonalData.Settings.VolumeMaster = ((float)masterToApply) / 10f;
            PersonalData.Settings.VolumeMusic = ((float)musicToApply) / 10f;
            PersonalData.Settings.VolumeSFX = ((float)sFXToApply) / 10f;
            PersonalData.Settings.VolumeAmbient = ((float)ambientToApply) / 10f;
        }
    }

    struct CustomVolumeCall
    {
        public Action<int> TargetMethod;
        public int newVolume;

        public void Activate()
        {
            TargetMethod?.Invoke(newVolume);
        }
    }
}
