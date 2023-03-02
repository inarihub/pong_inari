using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace pong_inari.engine
{
    class Game : MainWindow
    {
        private Canvas _gameRegion;
        public Game()
        {
            var mainwindow = App.Current.MainWindow as MainWindow;
        }
    }
}
