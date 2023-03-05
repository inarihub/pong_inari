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
        private Random _rngCreator;
        private Action BallBehaivor;
        public Sys.Timer FrameUpdater { get; private set; }

        public GameWindow GWin;
        public Dictionary<string, double> GameField { get; private set; }

        public PlayerBall Ball;
        public List<GameTarget> Targets { get; set; }

        public PlayerStick Stick;
        public ScoreBoard Scores { get; set; }

        private bool _isInProcess;
        public bool IsInProcess
        {
            get { return _isInProcess; }
            set { _isInProcess = value; OnStateChange.Invoke(this, value); }
        }
        private bool _isPlayerStarted;
        public bool IsPlayerStarted
        {
            get { return _isPlayerStarted; }
            set { _isPlayerStarted = value; OnPlayerStarted.Invoke(this, value); }
        }
        public event EventHandler<bool> OnStateChange;
        public event EventHandler<bool> OnPlayerStarted;
        public Game(GameWindow gWin)
        {
            _rngCreator = new Random(DateTime.UtcNow.Microsecond);
            GWin = gWin;
            GameField = new Dictionary<string, double>();
            SetGameFieled();
            Stick = new("PlayerStick", gWin.PlayerStick);
            Ball = new("PlayerBall", gWin.PlayerBall, Stick, this);
            Targets = new List<GameTarget>();

            FrameUpdater = new Sys.Timer();
            FrameUpdater.Interval = TimeSpan.FromMicroseconds(16700).TotalMilliseconds;
            FrameUpdater.AutoReset = true;

            OnStateChange += StateChangeHandler;
            IsInProcess = false;

            OnPlayerStarted += PlayerStartedHandler;
            IsPlayerStarted = false;

            Scores = new(gWin.PlayerScore);
        }
        private void SetGameFieled()
        {
            GameField.Add("Left", 50);
            GameField.Add("Right", 784);
            GameField.Add("Top", 16);
            GameField.Add("Bottom", 584);
        }
        private void PlayerStartedHandler(object? sender, bool isStarted)
        {
            if (isStarted)
            {
                FrameUpdater.Elapsed += GameStartedHandler;
                BallBehaivor += Ball.MoveBall;
                BallBehaivor -= Ball.AttachBall;
            }
        }
        private void StateChangeHandler(object? sender, bool IsStarting)
        {
            if (IsStarting)
            {
                BallBehaivor += Ball.AttachBall;
                FrameUpdater.Elapsed += GameInitializedHandler;
            }
        }
        private async void GameInitializedHandler(object? sender, Sys.ElapsedEventArgs e)
        {
            await Task.Run(() => GWin.Controls.MoveStick(Stick));
            await Task.Run(BallBehaivor);
        }
        private async void GameStartedHandler(object? sender, Sys.ElapsedEventArgs e)
        {
            var taskSpawnTargets = Task.Run(SpawnTarget);
            var taskScore = Task.Run(() => { Scores += 10; });
            await Task.WhenAll(taskSpawnTargets, taskScore);
        }
        public void Initialize()
        {
            IsInProcess = true;
            FrameUpdater.Start();
        }
        public void Start()
        {
            if (!IsInProcess)
            {
                IsInProcess = true;
            }
            if (IsPlayerStarted)
            {
                Ball.Unfreeze();
            }
            else
            {
                IsPlayerStarted = true;
                Ball.Start();
            }
            
        }
        public void Stop()
        {
            Ball.Freeze();
            IsInProcess = false;
            IsPlayerStarted = false;
            FrameUpdater.Stop();
        }
        private async Task SpawnTarget()
        {
            while (Targets.Count < Cfg.MaxTargets)
            {
                await GWin.GameRegion.Dispatcher.BeginInvoke(() =>
                {
                    var newTarget = new GameTarget("YellowCat", GameTarget.CreateTarget());
                    Targets.Add(newTarget);
                    GWin.GameRegion.Children.Add(newTarget.GameShape);
                    int leftBorder = Convert.ToInt32(GameField["Left"]);
                    int rightBorder = Convert.ToInt32(GameField["Right"] - newTarget.GameShape.Width);
                    int topBorder = Convert.ToInt32(GameField["Top"]);
                    int bottomBorder = Convert.ToInt32(GameField["Bottom"] - newTarget.GameShape.Height);
                    Canvas.SetLeft(newTarget.GameShape, _rngCreator.
                        Next(leftBorder, rightBorder));
                    Canvas.SetTop(newTarget.GameShape, _rngCreator.
                        Next(topBorder, bottomBorder));
                    newTarget.GameShape.Visibility = Visibility.Visible;
                });
            }
        }
        private Task RemoveTarget(GameTarget obj)
        {
            obj.GameShape.Visibility = Visibility.Hidden;
            Targets.Remove(obj);
            GWin.GameRegion.Children.Remove(obj.GameShape);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            GC.Collect();
        }
    }
}
