using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.Diagnostics;
using Sys = System.Timers;
using System.ComponentModel;
using System.Windows;
using Cfg = pong_inari.GameConfig;

namespace pong_inari.engine
{
    public class Game : IDisposable
    {
        private Action BallBehaivor;
        public Sys.Timer FrameUpdater { get; private set; }

        public GameWindow GWin;
        public GameElements Elements { get; private set; }
        public ScoreBoard Scores { get; set; }

        private bool _isInProcess = false;
        public bool IsInProcess
        {
            get { return _isInProcess; }
            set { _isInProcess = value; OnStateChange.Invoke(this, value); }
        }
        private bool _isPlayerPreparing = false;
        public bool IsPlayerPreparing
        {
            get { return _isPlayerPreparing; }
            set { _isPlayerPreparing = value; OnPlayerStarted.Invoke(this, value); }
        }
        public event EventHandler<bool> OnStateChange;
        public event EventHandler<bool> OnPlayerStarted;
        const double ONE_FRAME_DURATION = 16700;
        public Game(GameWindow gWin)
        {
            Elements = new GameElements(gWin);
            GWin = gWin;

            FrameUpdater = new Sys.Timer();
            FrameUpdater.Interval = TimeSpan.FromMicroseconds(ONE_FRAME_DURATION).TotalMilliseconds;
            FrameUpdater.AutoReset = true;

            OnStateChange += StateChangeHandler;
            OnPlayerStarted += PlayerStartedHandler;

            BallBehaivor += Elements.Ball.AttachBallToStick;
            Scores = new(gWin.PlayerScore);
        }

        private void PlayerStartedHandler(object? sender, bool isPreparing)
        {
            if (isPreparing)
            {
                FrameUpdater.Elapsed += ControlsActivatedHandler;
            }
            else
            {
                BallBehaivor -= Elements.Ball.AttachBallToStick;
                BallBehaivor += Elements.Ball.MoveBall;
                
                FrameUpdater.Elapsed -= ControlsActivatedHandler;
                Elements.Ball.Start();
            }
        }
        private void StateChangeHandler(object? sender, bool IsOn)
        {
            if (IsOn)
            {
                FrameUpdater.Elapsed += GameStartedHandler;
                FrameUpdater.Elapsed += ControlsActivatedHandler;
            }
            else
            {
                FrameUpdater.Elapsed -= GameStartedHandler;
                FrameUpdater.Elapsed -= ControlsActivatedHandler;
            }
        }
        private async void ControlsActivatedHandler(object? sender, Sys.ElapsedEventArgs e)
        {
            await Task.Run(() => GWin.Controls.MoveStick(Elements.Stick));
            await Task.Run(BallBehaivor);
        }
        private async void GameStartedHandler(object? sender, Sys.ElapsedEventArgs e)
        {
            await GWin.Dispatcher.BeginInvoke(() =>
            {
                if (Canvas.GetLeft(Elements.Ball.GameShape) < 0)
                {
                    SetGameOver();
                    return;
                }
            });
            var taskSpawnTargets = Task.Run(() => Elements.SpawnTarget(GWin));
            var taskScore = Task.Run(() => { Scores += 10; });
            await Task.WhenAll(taskSpawnTargets, taskScore);
        }
        public void Initialize()
        {
            IsPlayerPreparing = true;
            FrameUpdater.Start();
        }
        private void SetGameOver()
        {
            Stop();
            GWin.StartMessage.Content = "GAME OVER (Press \"Esc\" to exit)";
            GWin.StartMessage.HorizontalAlignment = HorizontalAlignment.Center;
            GWin.StartMessage.Visibility = Visibility.Visible;
        }
        public void Start()
        {
            if (IsPlayerPreparing)
            {
                Elements.Ball.Start();
                IsPlayerPreparing = false;
                IsInProcess = true;
                FrameUpdater.Start();
            }
            else
            {
                Elements.Ball.Unfreeze();
                IsInProcess = true;
                FrameUpdater.Start();
            }
        }
        public void Stop()
        {
            IsInProcess = false;
            FrameUpdater.Stop();
            Elements.Ball.Freeze();
        }
        public void Dispose()
        {
            GC.Collect();
        }
    }
}
