using System.Threading.Tasks;
using SharpDX.XAudio2;

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
            if (voice is null) { return Task.CompletedTask; }
            if (!audio.IsPlaying)
            {
                var volume = (float)((double)App.GlobalVolumePercent / 100);
                voice.SetVolume(volume);
                voice.Start();
                audio.IsPlaying = true;
            }          
            else if (audio.IsPlaying)
            {
                voice.Stop();
                voice.Start();
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