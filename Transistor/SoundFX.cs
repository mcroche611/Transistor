using System;
using System.Media;
// http://csharphelper.com/blog/2016/08/play-wav-files-in-c/
// https://docs.microsoft.com/es-es/dotnet/api/system.media.soundplayer.play?view=net-5.0
// https://www.reddit.com/r/csharp/comments/cvcl3z/cant_use_systemmedia_and_cant_find_correct_dll_to/

namespace Transistor
{
    class SoundFX
    {
        bool enabled = true;
        const string fxFolder = @"fx\";
        SoundPlayer sp = new SoundPlayer();
        SoundPlayer spHit = new SoundPlayer(fxFolder + "mixkit-martial-arts-punch-2052.wav");
        SoundPlayer spPing = new SoundPlayer(fxFolder + "mixkit-short-laser-gun-shot-1670.wav");
        SoundPlayer spCrash = new SoundPlayer(fxFolder + "mixkit-bit-war-suprise-item-3162.wav");
        SoundPlayer spLoad = new SoundPlayer(fxFolder + "mixkit-video-game-power-up-3164.wav");
        SoundPlayer spBreach = new SoundPlayer(fxFolder + "mixkit-sci-fi-laser-in-space-sound-2825.wav");
        SoundPlayer spTurn1 = new SoundPlayer(fxFolder + "mixkit-magic-glitter-shot-2353.wav");
        SoundPlayer spTurn2 = new SoundPlayer(fxFolder + "mixkit-retro-arcade-casino-notification-211.wav");
        SoundPlayer spExplosion = new SoundPlayer(fxFolder + "mixkit-fantasy-explosion-and-debris-1684.wav");

        public void PlayDamage()
        {
            if (enabled)
            {
                spHit.Play();
            }
        }

        public void PlayPing()
        {
            if (enabled)
            {
                spPing.Play();
            }
        }

        public void PlayLoad()
        {
            if (enabled)
            {
                spLoad.Play();
            }
        }

        public void PlayBreach()
        {
            if (enabled)
            {
                spBreach.Play();
            }
        }

        public void PlayCrash()
        {
            if (enabled)
            {
                spCrash.Play();
            }
        }

        public void PlayTurn1()
        {
            if (enabled)
            {
                spTurn1.Play();
            }
        }

        public void PlayTurn2()
        {
            if (enabled)
            {
                spTurn2.Play();
            }
        }

        public void PlayExplosion()
        {
            if (enabled)
            {
                spExplosion.Play();
            }
        }

        public void PlayNewLevel()
        {
            if (enabled)
            {
                sp.SoundLocation = fxFolder + "mixkit-arcade-game-complete-or-approved-mission-205.wav";
                sp.Play();
            }
        }

        public void PlayLevelCompleted()
        {
            if (enabled)
            {
                sp.SoundLocation = fxFolder + "mixkit-completion-of-a-level-2063.wav";
                sp.Play();
            }
        }

        public void PlayGameOver()
        {
            if (enabled)
            {
                sp.SoundLocation = fxFolder + "mixkit-arcade-retro-game-over-213.wav";
                sp.Play();
            }
        }

        public void PlayMenuIntro()
        {
            if (enabled)
            {
                sp.SoundLocation = fxFolder + "Crowander - In Action.wav";
                sp.Play();
            }
        }

        public void PlayOutro()
        {
            if (enabled)
            {
                sp.SoundLocation = fxFolder + "Soularflair - Left Unsaid-section 1.wav";
                sp.Play();
            }
        }
    }
}
