using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Cfg = pong_inari.GameConfig;

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
                await GWin.PlayerStick.Dispatcher.BeginInvoke(() =>
                {
                    stick.ChangeState(StickMoving.Down);
                    Canvas.SetTop(stick.GameShape, Canvas.GetTop(stick.GameShape) + stick.MoveVector.VelocityY);
                    
                });
            }
            else if (KeyPressed[Key.Up] && !KeyPressed[Key.Down])
            {
                await GWin.PlayerStick.Dispatcher.BeginInvoke(() =>
                {
                    stick.ChangeState(StickMoving.Up);
                    Canvas.SetTop(stick.GameShape, Canvas.GetTop(stick.GameShape) + stick.MoveVector.VelocityY);
                    
                });
            }
            else
            {
                GWin.PongGame.Stick.ChangeState(StickMoving.Static);
            }
        }
    }
}
