using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace pong_inari.engine
{
    public enum StickMoving
    {
        Up,
        Down,
        Static
    }
    public class GameControls
    {
        private GameWindow GWin { get; set; }
        public Dictionary<Key, bool> KeyPressed;
        public GameControls(GameWindow gWin) 
        { 
            GWin = gWin;
            KeyPressed = new Dictionary<Key, bool>();
            KeyPressed.Add(Key.Up, false);
            KeyPressed.Add(Key.Down, false);
        }
        public void SetInGameControls()
        {
            GWin.KeyDown += InGameKeyDownHandler;
            GWin.KeyUp += InGameKeyUpHandler;
        }
        public void UnsetInGameControls()
        {
            GWin.KeyDown -= InGameKeyDownHandler;
            GWin.KeyUp -= InGameKeyUpHandler;
        }
        private void InGameKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.S:
                case Key.Down:
                    KeyPressed[Key.Down] = true;
                    
                    break;
                case Key.W:
                case Key.Up:
                    KeyPressed[Key.Up] = true;
                    
                    break;
            }
        }
        private void InGameKeyUpHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.S:
                case Key.Down:
                    KeyPressed[Key.Down] = false;
    
                    break;
                case Key.W:
                case Key.Up:
                    KeyPressed[Key.Up] = false;
                    
                    break;
            }
        }
        public async Task MoveStick(PlayerStick stick)
        {
            if (KeyPressed[Key.Down] && !KeyPressed[Key.Up])
            {
                await MoveStickDown(stick);
            }
            else if (KeyPressed[Key.Up] && !KeyPressed[Key.Down])
            {
                await MoveStickUp(stick);
            }
            else
            {
                stick.ChangeState(StickMoving.Static);
            }
        }
        private async Task MoveStickDown(PlayerStick stick)
        {
            var stickStepPerFrame = stick.MoveVector.VelocityY;

            await GWin.PlayerStick.Dispatcher.BeginInvoke(() =>
            {
                var stickHeight = stick.GameShape.ActualHeight;
                var bottomBorder = GWin.PongGame.Elements.GameField["Bottom"] - stickHeight;

                if (Canvas.GetTop(stick.GameShape) + stickStepPerFrame >= bottomBorder)
                {
                    Canvas.SetTop(stick.GameShape, bottomBorder);
                    return;
                }
                stick.ChangeState(StickMoving.Down);
                Canvas.SetTop(stick.GameShape, Canvas.GetTop(stick.GameShape) + stickStepPerFrame);

            });
        }
        private async Task MoveStickUp(PlayerStick stick) 
        {
            var stickStepPerFrame = stick.MoveVector.VelocityY;

            await GWin.PlayerStick.Dispatcher.BeginInvoke(() =>
            {
                var topBorder = GWin.PongGame.Elements.GameField["Top"];

                if (Canvas.GetTop(stick.GameShape) + stickStepPerFrame <= topBorder)
                {
                    Canvas.SetTop(stick.GameShape, topBorder);
                    return;
                }
                stick.ChangeState(StickMoving.Up);
                Canvas.SetTop(stick.GameShape, Canvas.GetTop(stick.GameShape) + stickStepPerFrame);

            });
        }
    }
}
