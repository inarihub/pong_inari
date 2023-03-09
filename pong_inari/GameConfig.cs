using pong_inari.MainMenuPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace pong_inari
{
    public static class GameConfig
    {
        public static int CurrentSkin { get; set; }
        public static List<ImageSource?> BallSkins { get; set; }
        public static int SoundLevel { get; set; }
        public static DifficultyLevel Difficulty
        {
            get { return _diffLevel; }
            set
            {
                _diffLevel = value;
                SetDiffModifires();
            }
        }
        private static DifficultyLevel _diffLevel;
        public static double PlayerBallSpeed { get; set; }
        public static double PlayerStickSpeed { get; set; }
        public static int MaxTargets { get; set; }
        static GameConfig()
        {
            CurrentSkin = 0;
            BallSkins = new List<ImageSource?>();
            SkinInitialization();
            SoundLevel = 50;
            Difficulty = DifficultyLevel.Medium;
        }
        private static void SetDiffModifires()
        {
            switch (Difficulty)
            {
                case DifficultyLevel.Easy:
                    PlayerBallSpeed = 4;
                    PlayerStickSpeed = 4;
                    MaxTargets = 10;
                    break;
                case DifficultyLevel.Medium:
                    PlayerBallSpeed = 7;
                    PlayerStickSpeed = 7;
                    MaxTargets = 12;
                    break;
                case DifficultyLevel.Hard:
                    PlayerBallSpeed = 12;
                    PlayerStickSpeed = 12;
                    MaxTargets = 15;
                    break;
                default:
                    break;
            }
        }
        private static void SkinInitialization()
        {
            BallSkins.Add(App.Current.Resources["BallDefault"] as ImageSource);
            BallSkins.Add(App.Current.Resources["BallBlue"] as ImageSource);
            BallSkins.Add(App.Current.Resources["BallYellow"] as ImageSource);
        }
    }
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }
}
