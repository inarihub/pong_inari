using pong_inari.engine;
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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
        public string Name { get; set; }
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
            Name = name;
            UnmanagedMemoryStream? stream = resources.Sounds.ResourceManager.GetStream(name);
            if (stream is null) { throw new NullReferenceException($"There is no elements with name {name}"); }

            _soundStream = new(stream);
            _audioBuffer = new AudioBuffer
            {
                Stream = _soundStream.ToDataStream(),
                AudioBytes = (int)_soundStream.Length,
                Flags = BufferFlags.EndOfStream
            };

            if (isLooped) { _audioBuffer.LoopCount = 99; _audioBuffer.LoopBegin = 0; }
            

            Voice = new SourceVoice(GamePlayer.XAudio, _soundStream.Format, true);
            Voice.SubmitSourceBuffer(_audioBuffer, _soundStream.DecodedPacketsInfo);
            Voice.SetVolume(volume);
            Voice.StreamEnd += SoundEndsHandler;
        }

        private void SoundEndsHandler()
        {
            IsPlaying = false;
            _soundlib.Remove(Name);
            if (IsLooped)
            {
                this.PlayAsync();
            }
        }

        public static void SetGlobalVolume(float volume)
        {
            foreach (var soundBlock in _soundlib)
            {
                soundBlock.Value.Voice.SetVolume(volume);
                soundBlock.Value.Volume = volume;
            }
        }
        public static GameAudio GetAudio(string name, bool isLooped = false)
        {
            lock (_soundlib)
            {
                GameAudio? result = null;
                var volume = (float)((double)App.GlobalVolumePercent / 100);
                result = new GameAudio(name, volume, isLooped);
                var keyName = name;
                while (_soundlib.ContainsKey(keyName))
                {
                    keyName += new Random().Next(0, 9);
                }
                result = new GameAudio(name, volume, isLooped);
                _soundlib.Add(keyName, result);
                return result;
            }
        }
    }
}
