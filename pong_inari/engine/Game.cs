using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using Sys = System.Timers;
using System.Windows;
using Cfg = pong_inari.GameConfig;
using System.Timers;

namespace pong_inari.engine
{
    public class Game : IDisposable
    {
        private Action? _ballBehaivor;
        private GameState _currentState;
        public GameWindow _winOwner;
        public Sys.Timer FrameUpdater { get; private set; }
        public GameElements Elements { get; private set; }
        public ScoreBoard Score { get; set; }
        public GameState CurrentState
        {
            get { return _currentState; }
            set { OnStatesChanged[value].Invoke(); _currentState = value; }
        }
        public Dictionary<GameState, Action> OnStatesChanged { get; private set; }

        private const double ONE_FRAME_DURATION = 16700;
        public Game(GameWindow gWin)
        {
            _winOwner = gWin;
            Elements = new GameElements(_winOwner);

            OnStatesChanged = new Dictionary<GameState, Action>
            {
                { GameState.Preparing, PreparingHandler },
                { GameState.Started, StartGameHandler },
                { GameState.Unpaused, UnpauseHandler },
                { GameState.Paused, PauseHandler },
                { GameState.GameOver, GameOverHandler },
                { GameState.Exited, ExitGameHandler },
            };

            FrameUpdater = new Sys.Timer
            {
                Interval = TimeSpan.FromMicroseconds(ONE_FRAME_DURATION).TotalMilliseconds,
                AutoReset = true
            };

            Score = new(_winOwner.PlayerScore);
        }
        private void PreparingHandler()
        {
            FrameUpdater.Elapsed += ControlsActivatedHandler;
            _ballBehaivor += Elements.Ball.AttachBallToStick;
            FrameUpdater.Start();
            var startMsg = _winOwner.StartMessage;
            startMsg.Visibility = Visibility.Visible;
            startMsg.Content = "Are you ready? Press \"Enter\" to start!";
        }
        private void StartGameHandler()
        {
            _ballBehaivor -= Elements.Ball.AttachBallToStick;
            _ballBehaivor += Elements.Ball.MoveBall;
            FrameUpdater.Elapsed += GameStartedHandler;
            Elements.Ball.Start();
            var startMsg = _winOwner.StartMessage;
            startMsg.Visibility = Visibility.Hidden;
        }
        private void UnpauseHandler()
        {
            Elements.Ball.Unfreeze();
            FrameUpdater.Elapsed += GameStartedHandler;
            FrameUpdater.Elapsed += ControlsActivatedHandler;
            FrameUpdater.Start();
            var startMsg = _winOwner.StartMessage;
            startMsg.Visibility = Visibility.Hidden;
        }
        private void PauseHandler()
        {
            Elements.Ball.Freeze();
            FrameUpdater.Stop();
            FrameUpdater.Elapsed -= GameStartedHandler;
            FrameUpdater.Elapsed -= ControlsActivatedHandler;
            var startMsg = _winOwner.StartMessage;
            startMsg.Content = "PAUSE (Enter - continue, Esc - Main menu)";
            startMsg.Visibility = Visibility.Visible;
        }
        private void GameOverHandler()
        {
            FrameUpdater.Stop();
            FrameUpdater.Elapsed -= GameStartedHandler;
            FrameUpdater.Elapsed -= ControlsActivatedHandler;
            var startMsg = _winOwner.StartMessage;
            startMsg.Content = "GAME OVER (Press \"Esc\" to exit)";
            startMsg.HorizontalAlignment = HorizontalAlignment.Center;
            startMsg.Visibility = Visibility.Visible;
        }
        private void ExitGameHandler()
        {
            Dispose();
        }
        private async void ControlsActivatedHandler(object? sender, Sys.ElapsedEventArgs e)
        {
            await Task.Run(() => _winOwner.Controls.MoveStick(Elements.Stick));
            if (_ballBehaivor is null) { throw new NullReferenceException(nameof(ControlsActivatedHandler)); }
            await Task.Run(_ballBehaivor);
        }
        private async void GameStartedHandler(object? sender, Sys.ElapsedEventArgs e)
        {
            await _winOwner.Dispatcher.BeginInvoke(() =>
            {
                if (Canvas.GetLeft(Elements.Ball.GameShape) < 0)
                {
                    CurrentState = GameState.GameOver;
                    return;
                }
            });
            var taskSpawnTargets = Task.Run(() => Elements.SpawnTarget(_winOwner));
            var taskScore = Task.Run(() => { Score += 10; });
            await Task.WhenAll(taskSpawnTargets, taskScore);
        }
        public void Dispose()
        {
            if (FrameUpdater.Enabled)
            {
                FrameUpdater.Stop();
            }
            FrameUpdater.Dispose();
            
            GC.Collect();
        }
    }
    public enum GameState
    {
        Preparing,
        Started,
        Unpaused,
        Paused,
        GameOver,
        Exited
    }
}
