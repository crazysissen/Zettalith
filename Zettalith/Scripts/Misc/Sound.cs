using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Zettalith
{
    static class Sound
    {
        public static float SFXVolume { get; private set; }
        public static float MusicVolume { get; private set; }

        private static List<SoundEffectInstance> playingEffects;
        private static SoundEffectInstance playingSong;

        public static void Init()
        {
            playingEffects = new List<SoundEffectInstance>();
        }

        public static void Update()
        {
            for (int i = playingEffects.Count; i >= 0; --i)
            {
                if (playingEffects[i].State == SoundState.Stopped)
                {
                    playingEffects.RemoveAt(i);
                }
            }
        }

        public static void SetSFXVolume(float volume)
        {
            SFXVolume = volume;

            foreach (SoundEffectInstance effect in playingEffects)
            {
                effect.Volume = volume;
            }
        }

        public static void SetMusicVolume(float volume)
        {
            MusicVolume = volume;

            playingSong.Volume = volume;
        }

        public static void PlaySong(SoundEffect effect)
        {
            StopSong();

            playingSong = effect.CreateInstance();
            playingSong.IsLooped = true;
            playingSong.Volume = PersonalData.Settings.VolumeMusic;
            playingSong.Play();
        }

        public static void StopSong()
        {
            playingSong?.Stop();
            playingSong = null;
        }

        public static void PauseAllEffects()
        {
            foreach (SoundEffectInstance effect in playingEffects)
            {
                effect.Pause();
            }
        }

        public static void ResumeAllEffects()
        {
            foreach (SoundEffectInstance effect in playingEffects)
            {
                effect.Resume();
            }
        }
    }
}
