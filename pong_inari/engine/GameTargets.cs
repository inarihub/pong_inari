using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace pong_inari.engine
{
    public class GameTarget : GameObject
    {
        private static int _nameId;
        public GameTarget(string name, Shape shape) : base(name, shape)
        {
            
        }
        public static Shape CreateTarget()
        {
            var gameShape = new Rectangle()
            {
                Fill = new ImageBrush(App.Current.Resources["CatImage"] as ImageSource),
                Height = 30,
                Width = 30,
                Visibility = Visibility.Hidden,
                Name = "target" + _nameId.ToString()
            };
            _nameId++;
            return gameShape;
        }
    }
}
