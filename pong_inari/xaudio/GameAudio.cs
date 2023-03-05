using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace pong_inari.xaudio
{
    public class GameAudio
    {
        private static Dictionary<string, GameAudio> _soundlib = new Dictionary<string, GameAudio>();
        public SoundStream _soundStream;

        private AudioBuffer _audioBuffer;
        /// <summary>
        /// Source voice to play.
        /// </summary>
        public SourceVoice Voice { get; private set; }
        /// <summary>
        /// Volume level of current audio clip.
        /// </summary>
        public float Volume { get; set; }
        /// <summary>
        /// Indicates is current voice playing.
        /// </summary>
        public bool IsPlaying { get; set; }
        /// <summary>
        /// Indicates is audio clip looped forever or intended to play once.
        /// </summary>
        public bool IsLooped { get; private set; }
        private GameAudio(string name, float volume, bool isLooped)
        {
            IsLooped = isLooped;
            Volume = volume;

            UnmanagedMemoryStream? stream = resources.Sounds.ResourceManager.GetStream(name);
            if (stream is null) { throw new NullReferenceException($"There is no elements with name {name}"); }

            _soundStream = new(stream);
            _audioBuffer = new AudioBuffer
            {
                Stream = _soundStream.ToDataStream(),
                AudioBytes = (int)_soundStream.Length,
                Flags = BufferFlags.EndOfStream
            };
            
            if (isLooped) { _audioBuffer.LoopCount = 99; }

            Voice = new SourceVoice(GamePlayer.XAudio, _soundStream.Format, true);
            Voice.SubmitSourceBuffer(_audioBuffer, _soundStream.DecodedPacketsInfo);
        }
        public static GameAudio GetAudio(string name, float volume = 1.0f, bool isLooped = false)
        {
            lock (_soundlib)
            {
                GameAudio? result = null;

                if (_soundlib.ContainsKey(name))
                {
                    result = _soundlib[name];
                }
                else
                {
                    result = new GameAudio(name, volume, isLooped);

                }
                return result;
            }
        }
    }
}
