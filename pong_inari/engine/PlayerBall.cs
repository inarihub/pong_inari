using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Cfg = pong_inari.GameConfig;

namespace pong_inari.engine
{
    public class PlayerBall : GameObject
    {
        public EllipseGeometry _ballGeometry;
        public System.Windows.Point _ballCenter;
        private Acceleration _previousAcc;
        private GameElements Elements { get; set; } public Acceleration MoveVector { get; set; }
        public PlayerBall(string name, Shape obj, GameElements elements) : base(name, obj, true)
        {
            _ballCenter = new System.Windows.Point(CenterX, CenterY);
            _ballGeometry = new EllipseGeometry(_ballCenter, 15, 15);
            IsMoving = false;
            MoveVector = new(0, 0);
            _previousAcc = new(0, 0);
            Elements = elements;
        }
        public void Start()
        {
            Unfreeze();
            IsMoving = true;
            MoveVector.VelocityX = Cfg.PlayerBallSpeed;
        }
        public async void MoveBall()
        {
            if (MoveVector.VelocityX > Cfg.PlayerBallSpeed)
            {
                MoveVector.VelocityX = Cfg.PlayerBallSpeed;
            }
            if (MoveVector.VelocityY > Cfg.PlayerBallSpeed)
            {
                MoveVector.VelocityY = Cfg.PlayerBallSpeed;
            }
            await GameShape.Dispatcher.BeginInvoke(() =>
            {
                _ballCenter.X = Canvas.GetLeft(GameShape) + (GameShape.Width / 2);
                _ballCenter.Y = Canvas.GetTop(GameShape) + (GameShape.Height / 2);
                _ballGeometry.Center = _ballCenter;
                Elements.CollisionModels.CheckCollisionModels(_ballGeometry);
                Canvas.SetLeft(GameShape, Canvas.GetLeft(GameShape) + MoveVector.VelocityX);
                Canvas.SetTop(GameShape, Canvas.GetTop(GameShape) + MoveVector.VelocityY);
            });
        }
        public async void AttachBallToStick()
        {
            var stick = Elements.Stick;
            MoveVector.VelocityY = stick.MoveVector.VelocityY;

            await GameShape.Dispatcher.BeginInvoke(() =>
            {
                Canvas.SetTop(GameShape, Canvas.GetTop(stick.GameShape) + (stick.GameShape.ActualHeight / 2) - (GameShape.ActualHeight / 2));
            });
        }
        public void Freeze()
        {
            _previousAcc.SetAcceleration(MoveVector.VelocityX, MoveVector.VelocityY);
            MoveVector.SetAcceleration(0, 0);
            IsMoving = false;
        }
        public void Unfreeze()
        {
            if (_previousAcc.VelocityX == 0 && _previousAcc.VelocityY == 0) { return; }
            IsMoving = true;
            MoveVector.SetAcceleration(_previousAcc.VelocityX, _previousAcc.VelocityY);
        }
    }
}
