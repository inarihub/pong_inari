using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace pong_inari.engine
{
    public class GameGeometry
    {
        private GameWindow GWin { get; set; }
        PlayerBall Ball { get; set; }
        public Dictionary<string, Geometry> CurrentGeometry { get; set; }
        private List<DependencyObject> Collision { get; set; }
        public GameGeometry(GameWindow gWin, PlayerBall ball)
        {
            GWin = gWin;
            Ball = ball;
            Collision = new List<DependencyObject>();
            CurrentGeometry = new Dictionary<string, Geometry>();
            CurrentGeometry.Add("PlayerBall", ball._ballGeometry);
        }
        public void CheckCollisionModels(Geometry obj)
        {
            var test = new GeometryHitTestParameters(obj);
            var resultCallback = new HitTestResultCallback(result => HitTestResultBehavior.Continue);
            var filterCallback = new HitTestFilterCallback(element =>
            {
                if (VisualTreeHelper.GetParent(element) == GWin.GameRegion && element.GetType() == typeof(Rectangle))
                {
                        Collision.Add(element);
                }

                return HitTestFilterBehavior.Continue;
            });
            VisualTreeHelper.HitTest(GWin.GameRegion, filterCallback, resultCallback, test);

            lock (Collision)
            {
                CollisionCheck();
            }
        }
        private void CollisionCheck()
        {
            if (!Collision.Any()) { return; }
            {
                string name;
                foreach (var collision in Collision)
                {
                    name = collision.GetValue(Rectangle.NameProperty) as string;

                    if (name == "YellowCat")
                    {
                        collision.SetValue(Rectangle.VisibilityProperty, Visibility.Hidden);
                        ReflectBall(collision as Rectangle);

                        Task.Run(() => GWin.PlaySoundEffect("cathit"));
                    }
                    else if (name.Contains("Line"))
                    {
                        ReflectBall(collision as Rectangle);
                        Task.Run(() => GWin.PlaySoundEffect("playerhit"));
                    }
                    else if (name.Contains("Stick"))
                    {
                        ReflectBall(collision as Rectangle);
                        Ball.MoveVector.VelocityY += (GWin.PongGame.Elements.Stick.MoveVector.VelocityY / 5);
                        Task.Run(() => GWin.PlaySoundEffect("playerhit"));
                    }
                }
                Collision.Clear();
            }
        }
        private void ReflectBall(Shape obj)
        {
            var objLeft = Canvas.GetLeft(obj);
            var objTop = Canvas.GetTop(obj);

            if (Ball.CenterX < objLeft || Ball.CenterX > objLeft + obj.ActualWidth)
            {
                Ball.MoveVector.VelocityX = -Ball.MoveVector.VelocityX;
            }
            if (Ball.CenterY < objTop || Ball.CenterY > objTop + obj.ActualHeight)
            {
                Ball.MoveVector.VelocityY = -Ball.MoveVector.VelocityY;
            }
        }
    }
}
