using System.Threading.Tasks;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System.Threading;
using System.Runtime.CompilerServices;

namespace pong_inari.xaudio
{
    public static class GamePlayer
    {
        public static XAudio2 XAudio { get; }
        private static MasteringVoice _master;
        static GamePlayer()
        {
            XAudio = new XAudio2();
            _master = new MasteringVoice(XAudio);
        }
        public static Task PlayAsync(this GameAudio audio)
        {
            var voice = audio.Voice;
            if (!audio.IsPlaying || voice is not null)
            {
                voice.SetVolume(audio.Volume);
                voice.Start();
                audio.IsPlaying = true;
            }          
            return Task.CompletedTask;
        }
        public static Task StopAsync(this GameAudio audio)
        {
            var voice = audio.Voice;
            if (voice is not null && audio.IsPlaying)
            {
                voice.Stop();
                audio.IsPlaying = false;
            }
            return Task.CompletedTask;
        }
    }
}