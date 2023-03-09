using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Cfg = pong_inari.GameConfig;

namespace pong_inari.engine
{
    public class GameElements
    {
        private Random _rngCreator;
        private GameWindow GWin { get; set; }
        public PlayerBall Ball { get; set; }
        public PlayerStick Stick { get; set; }
        public List<GameTarget> Targets { get; private set; }
        public GameGeometry CollisionModels { get; private set; }
        public Dictionary<string, double> GameField { get; private set; }
        public GameElements(GameWindow gWin)
        {
            _rngCreator = new Random(DateTime.UtcNow.Microsecond);
            GWin = gWin;
            Targets = new List<GameTarget>();
            GameField = new Dictionary<string, double>();
            Stick = new("PlayerStick", gWin.PlayerStick);
            Ball = new("PlayerBall", gWin.PlayerBall, this);
            CollisionModels = new(gWin, Ball);
            SetGameFieled();
        }
        private void SetGameFieled()
        {
            GameField.Add("Left", 50);
            GameField.Add("Right", 784);
            GameField.Add("Top", 16);
            GameField.Add("Bottom", 584);
        }
        public async Task SpawnTarget(GameWindow gWin)
        {
            GameTarget newTarget;
            int leftBorder, rightBorder, topBorder, bottomBorder;

            while (Targets.Count < Cfg.MaxTargets)
            {
                await gWin.GameRegion.Dispatcher.BeginInvoke(() =>
                {
                    newTarget = new GameTarget("YellowCat", GameTarget.CreateTarget());
                    Targets.Add(newTarget);
                    gWin.GameRegion.Children.Add(newTarget.GameShape);
                    leftBorder = Convert.ToInt32(GameField["Left"]);
                    rightBorder = Convert.ToInt32(GameField["Right"] - newTarget.GameShape.Width);
                    topBorder = Convert.ToInt32(GameField["Top"]);
                    bottomBorder = Convert.ToInt32(GameField["Bottom"] - newTarget.GameShape.Height);
                    Canvas.SetLeft(newTarget.GameShape, _rngCreator.
                        Next(leftBorder, rightBorder));
                    Canvas.SetTop(newTarget.GameShape, _rngCreator.
                        Next(topBorder, bottomBorder));
                    newTarget.GameShape.IsVisibleChanged += KillTargetHandler;
                    newTarget.GameShape.Visibility = Visibility.Visible;
                });
            }
        }

        private async void KillTargetHandler(object sender, DependencyPropertyChangedEventArgs isAlive)
        {
            var wasKilled = !(bool)isAlive.NewValue;

            if (wasKilled)
            {
                await RemoveTarget(GWin, sender as Shape);
            }
        }

        public async Task RemoveTarget(GameWindow gWin, Shape obj)
        {
            Targets.RemoveAll(x => x.GameShape.GetHashCode() == obj.GetHashCode());
            await GWin.Dispatcher.BeginInvoke(new Action(() =>
            {
                gWin.GameRegion.Children.Remove(obj);
            }));

        }
    }
}
