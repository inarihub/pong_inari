using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace pong_inari.uicontrols
{
    public class MenuSelector : IDisposable
    {
        public Image Item { get; set; }
        public MenuSelector()
        {
            Item = new Image();
            Item.Height = 30;
            Item.Width = 30;
            Item.Source = App.Current.Resources["CatImage"] as ImageSource;
        }
        void IDisposable.Dispose()
        {
            Item.Visibility = System.Windows.Visibility.Collapsed;
            Item.Source = null;
        }
    }
}
