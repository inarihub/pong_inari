using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Cfg = pong_inari.GameConfig;

namespace pong_inari.engine
{
    public class PlayerBall : GameObject
    {
        private Game CurrentGame { get; set; }
        private PlayerStick Stick { get; set; }

        private Acceleration _previousAcc;
        public Acceleration MoveVector { get; set; }
        private Line Path { get; set; }
        public PlayerBall(string name, Shape obj, PlayerStick stick, Game game) : base(name, obj, true)
        {
            CurrentGame = game;
            Path = new Line();
            Stick = stick;
            Path.Visibility = Visibility.Hidden;
            IsMoving = false;
            MoveVector = new(0, 0);
            _previousAcc = new(0, 0);
        }
        public void Start()
        {
            //PathGeometry path = new PathGeometry();
            //FigureStructure
            //System.Windows.Shapes.Line line = new();
            //line.TranslatePoint()
            Unfreeze();
            IsMoving = true;
            MoveVector.VelocityX = Cfg.PlayerBallSpeed;
            //OnHit += OnHitHandler;
        }

        //private async void OnHitHandler(object? sender, object? e)
        //{
        //     await Task.Run(() =>
        //     {
        //         if (e is null) { return; }
        //         MoveVector.Reflect(this, e as GameObject);
        //     });
        //}
        private async Task UpdatePath()
        {
            if (IsMoving)
            {
                Path.
            }
        }
        public async void MoveBall()
        {
            await GameShape.Dispatcher.BeginInvoke(() =>
            {
                Canvas.SetLeft(GameShape, Canvas.GetLeft(GameShape) + MoveVector.VelocityX);
                Canvas.SetTop(GameShape, Canvas.GetTop(GameShape) + MoveVector.VelocityY);
            });
            CheckIntersection();
        }
        public async void AttachBall()
        {
            MoveVector.VelocityY = Stick.MoveVector.VelocityY;

            await GameShape.Dispatcher.BeginInvoke(() =>
            {
                Canvas.SetTop(GameShape, Canvas.GetTop(GameShape) + MoveVector.VelocityY);
            });
        }
        public Task CheckIntersection()
        {
            
            return Task.CompletedTask;
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

            MoveVector.SetAcceleration(_previousAcc.VelocityX, _previousAcc.VelocityY);
        }
    }
}
