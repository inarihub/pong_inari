using pong_inari.engine;
using pong_inari.xaudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Cfg = pong_inari.GameConfig;

namespace pong_inari
{
    public partial class GameWindow : Window
    {
        const double PLAYERBALL_X_DEFAULT = 20;
        const double PLAYERBALL_Y_DEFAULT = 285;

        const double PLAYERSTICK_X_DEFAULT = 0;
        const double PLAYERSTICK_Y_DEFAULT = 255;
        public Game PongGame { get; set; }
        public GameControls Controls { get; set; }
        public GameWindow()
        {
            InitializeComponent();
            KeyDown += GameScreen_KeyDown;
            Controls = new(this);
            PlayerBall.Fill = new ImageBrush(Cfg.BallSkins[Cfg.CurrentSkin]);
            PlayerBall.Visibility = Visibility.Visible;
        }
        private void ResetWindow()
        {
            Hint.Visibility = Visibility.Hidden;
            ResetPlayer();
            PlayerScore.Content = "000000000000000";
            StartMessage.Visibility = Visibility.Visible;
        }
        private void ResetPlayer()
        {
            Canvas.SetLeft(PlayerBall, PLAYERBALL_X_DEFAULT);
            Canvas.SetTop(PlayerBall, PLAYERBALL_Y_DEFAULT);
            Canvas.SetLeft(PlayerStick, PLAYERSTICK_X_DEFAULT);
            Canvas.SetTop(PlayerStick, PLAYERSTICK_Y_DEFAULT);
        }
        public static async Task PlaySoundEffect(string name)
        {
            await GameAudio.GetAudio(name).PlayAsync();
        }
        private void GameScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) { return; }

            switch (e.Key)
            {
                case Key.Enter:
                    EnterAction();
                    break;
                case Key.Escape:
                    EscapeAction();
                    break;
            }
        }
        private void EnterAction()
        {
            if (PongGame is null) { throw new NullReferenceException(); }

            if (PongGame.CurrentState == GameState.Preparing)
            {
                PongGame.CurrentState = GameState.Started;
            }
            else if (PongGame.CurrentState == GameState.Paused)
            {
                PongGame.CurrentState = GameState.Unpaused;
            }
        }
        private void EscapeAction()
        {
            if (PongGame is null) { throw new NullReferenceException(); }

            if (PongGame.CurrentState == GameState.Started ||
                PongGame.CurrentState == GameState.Unpaused)
            {
                PongGame.CurrentState = GameState.Paused;
            }
            else
            {
                ResetWindow();
                Hide();
                PongGame.CurrentState = GameState.Exited;
                App.Current.MainWindow.Show();
            }
        }
        private void GameScreen_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var isVisible = (bool)e.NewValue;
            if (isVisible)
            {
                PongGame = new Game(this);
                PlayerBall.Fill = new ImageBrush(Cfg.BallSkins[Cfg.CurrentSkin]);
                PlayerBall.Visibility = Visibility.Visible;
                Hint.Visibility = Visibility.Visible;
                Focus();
                PongGame.CurrentState = GameState.Preparing;
                Controls.SetInGameControls();
            }
            else
            {
                Controls.UnsetInGameControls();
            }
        }
        private void GameScreen_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
