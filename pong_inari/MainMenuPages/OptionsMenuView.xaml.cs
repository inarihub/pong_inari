using pong_inari.uicontrols;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;

namespace pong_inari.MainMenuPages
{
    /// <summary>
    /// Interaction logic for OptionsMenuView.xaml
    /// </summary>
    public partial class OptionsMenuView : Page, IGameMenu
    {
        public Grid MenuGrid { get; set; }
        public MenuControl MenuControl { get; set; }

        const int VOLUME_STEP = 10;
        public OptionsMenuView()
        {
            InitializeComponent();
            SetVolumeLabel();
            SetDefaultDifficulty();
            MenuGrid = OptionsMenuGrid;
            MenuControl = new(MenuGrid);
            InitializeOptions();
        }
        private void SetDefaultDifficulty()
        {
            DiffChanger.Content = Enum.GetName(typeof(DifficultyLevel), GameConfig.Difficulty);
        }
        private void SetVolumeLabel()
        {
            ((App)App.Current).BackgroundAudio.Voice.GetVolume(out float volume);
            SoundChanger.Content = ((int)(volume * 100)).ToString();
        }
        private void SetDefaultSkin()
        {
            SkinChanger.Source = GameConfig.BallSkins[GameConfig.CurrentSkin] ?? throw new NullReferenceException(nameof(GameConfig.BallSkins));
        }
        private void InitializeOptions()
        {
            MenuControl.OptionsList.Add(new GameOption(Difficulty, DifficultyChange, true, DiffChanger));
            MenuControl.OptionsList.Add(new GameOption(Sound, SoundChange, true, SoundChanger));
            MenuControl.OptionsList.Add(new GameOption(BallSkin, SkinChange, true, SkinChanger));
            MenuControl.OptionsList.Add(new GameOption(ExitOptions, ExitMenu));
            MenuControl.SetMouseFeedback();
        }
        private void DifficultyChange(Switcher dir)
        {
            int currentDif = (int)GameConfig.Difficulty;
            currentDif = (dir == Switcher.Left) ? currentDif - 1 : currentDif + 1;

            int diffNumber = typeof(DifficultyLevel).GetEnumNames().Length;
            if (currentDif < 0) { currentDif = diffNumber - 1; }
            else if (currentDif > diffNumber - 1) { currentDif = 0; }

            GameConfig.Difficulty = (DifficultyLevel)currentDif;
            DiffChanger.Content = Enum.GetName(typeof(DifficultyLevel), currentDif);
        }
        private void SoundChange(Switcher dir)
        {
            ((App)App.Current).BackgroundAudio.Voice.GetVolume(out float volume);
            int volumePercent = (int)(volume * 100);
            volumePercent = (dir == Switcher.Left) ? volumePercent - VOLUME_STEP : volumePercent + VOLUME_STEP;

            if (volumePercent < 0)
            {
                volumePercent = 0;
            }
            else if (volumePercent > 100)
            {
                volumePercent = 100;
            }

            ((App)App.Current).BackgroundAudio.Voice.SetVolume(volumePercent / 100f);
            SoundChanger.Content = volumePercent;
        }
        private void SkinChange(Switcher dir)
        {
            int nextSkinIndex = (dir == Switcher.Left) ? GameConfig.CurrentSkin - 1 : GameConfig.CurrentSkin + 1;
            if (nextSkinIndex < 0) { nextSkinIndex= GameConfig.BallSkins.Count - 1; }
            else if (nextSkinIndex > GameConfig.BallSkins.Count - 1) { nextSkinIndex = 0;}
            GameConfig.CurrentSkin = nextSkinIndex;
            SkinChanger.Source = GameConfig.BallSkins[GameConfig.CurrentSkin];
        }
        private void ExitMenu(Switcher dir)
        {
            ((MainWindow)App.Current.MainWindow).ChangeMenu("StartMenu");
        }
    }
}
