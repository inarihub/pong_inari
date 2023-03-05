using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Cfg = pong_inari.GameConfig;

namespace pong_inari.engine
{
    public class PlayerStick : GameObject
    {
        public double PlayerSpeed { get; set; }
        public Acceleration MoveVector { get; set; }
        public PlayerStick(string name, Shape obj) : base(name, obj, true)
        {
            PlayerSpeed = Cfg.PlayerStickSpeed;
            MoveVector = new(0, 0);
        }
        internal void ChangeState(StickMoving value)
        {
            if (value == StickMoving.Up)
            {
                MoveVector.VelocityY = -PlayerSpeed;
                IsMoving = true;
            }
            else if (value == StickMoving.Down)
            {
                MoveVector.VelocityY = PlayerSpeed;
                IsMoving = true;
            }
            else
            {
                MoveVector.VelocityY = 0;
                IsMoving = false;
            } 
        }
    }
}
