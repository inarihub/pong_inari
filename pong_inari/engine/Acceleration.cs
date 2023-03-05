using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace pong_inari.engine
{
    public class Acceleration
    {
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        private int Angle { get; set; }
        public Acceleration(double velocityX, double velocityY) 
        {
            VelocityX= velocityX;
            VelocityY= velocityY;
        }
        public void SetAcceleration(double velocityX, double velocityY)
        {
            VelocityX= velocityX;
            VelocityY = velocityY;
        }
        public Task Reflect(PlayerBall playerBall, GameObject gameObject)
        {
            if (gameObject.GameShape is not Ellipse)
            {
               
            }
            return Task.CompletedTask;
        }
    }
}
