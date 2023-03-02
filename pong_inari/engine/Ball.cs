using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace pong_inari.engine
{
    class Ball/* : IGameObject, ICollision*/
    {
        
        public Task BallStart()
        {
            MainWindow window = Window.GetWindow(App.Current.MainWindow) as MainWindow;
            return Task.CompletedTask;
        }
    }
}
