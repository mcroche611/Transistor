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
        SoundPlayer sp1 = new SoundPlayer(fxFolder + "mixkit-boxer-getting-hit-2055.wav");
        SoundPlayer sp2 = new SoundPlayer(fxFolder + "mixkit-martial-arts-punch-2052.wav");
        SoundPlayer spd1 = new SoundPlayer(fxFolder + "mixkit-short-laser-gun-shot-1670.wav");
        //SoundPlayer sp2 = new SoundPlayer(fxFolder + "mixkit-boxer-getting-hit-2055.wav");

        public void PlayDamage1()
        {
            if (enabled)
            {
                sp1.Play();
            }
            
        }

        public void PlayDamage2()
        {
            if (enabled)
            {
                sp2.Play();
            }
        }

        public void PlayShot1()
        {
            if (enabled)
            {
                spd1.Play();
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
                sp.SoundLocation = fxFolder + "ColorofMusic.wav";
                sp.Play();
            }
        }

    }
}
