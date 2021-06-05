using System;
using System.Media;
// http://csharphelper.com/blog/2016/08/play-wav-files-in-c/
// https://docs.microsoft.com/es-es/dotnet/api/system.media.soundplayer.play?view=net-5.0
// https://www.reddit.com/r/csharp/comments/cvcl3z/cant_use_systemmedia_and_cant_find_correct_dll_to/

namespace Transistor
{
    class SoundFX
    {
        bool enabled = false;
        const string fxFolder = @"fx\";
        SoundPlayer sp = new SoundPlayer();
        //SoundPlayer sp2 = new SoundPlayer(fxFolder + "mixkit-boxer-getting-hit-2055.wav");

        public void PlayDamage1()
        {
            if (enabled)
            {
                sp.SoundLocation = fxFolder + "mixkit-boxer-getting-hit-2055.wav";
                sp.Play();
            }
            
        }

        public void PlayDamage2()
        {
            if (enabled)
            {
                sp.SoundLocation = fxFolder + "mixkit-martial-arts-punch-2052.wav";
                sp.Play();
            }
        }

        public void PlayShot1()
        {
            if (enabled)
            {
                sp.SoundLocation = fxFolder + "mixkit-short-laser-gun-shot-1670.wav";
                sp.Play();
            }
        }
    }
}
