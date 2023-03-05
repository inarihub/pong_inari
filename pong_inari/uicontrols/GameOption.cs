using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace pong_inari.uicontrols
{
    public delegate void OptionAction(Switcher dir);
    public class GameOption
    {
        public Label MenuLabel { get; set; }
        public OptionAction OptionActivation;
        public bool IsSwitchable { get; set; }
        public bool IsSelected { get; set; }
        public object? DependencyObject { get; set; }
        public GameOption(Label text, OptionAction action, bool isSwitchable = false, object? dependencyObj = null)
        {
            MenuLabel = text;
            OptionActivation = action;
            IsSwitchable = isSwitchable;
            IsSelected = false;
            DependencyObject = dependencyObj;
        }
    }
}
