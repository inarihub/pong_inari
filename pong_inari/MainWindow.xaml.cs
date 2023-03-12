using pong_inari.MainMenuPages;
using pong_inari.uicontrols;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace pong_inari
{
    public partial class MainWindow : Window
    {
        public Dictionary<string, IGameMenu> _menuPages;
        public string _current;
        public MainWindow()
        {
            InitializeComponent();
            _menuPages = new Dictionary<string, IGameMenu>
            {
                { "StartMenu", new StartMenuView() },
                { "OptionsMenu", new OptionsMenuView() }
            };
            _current = "StartMenu";
            this.ContentRendered += PageRenderedHandler;
        }
        private void PageRenderedHandler(object? sender, EventArgs e)
        {
            ChangeMenu(_current);
        }
        public void ChangeMenu(string menu)
        {
            _menuPages[_current].MenuControl.SetSelection();
            _current = menu;
            Content = _menuPages[menu];
        }
        public void KeyDownHandler(object sender, KeyEventArgs e)
        {
            var currentMenu = _menuPages[_current].MenuControl;

            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    currentMenu.MoveUp();
                    break;

                case Key.Down:
                case Key.S:
                    currentMenu.MoveDown();
                    break;

                case Key.Left:
                case Key.A:
                    currentMenu.ReadOptions(Switcher.Left);
                    break;

                case Key.Right:
                case Key.D:
                    currentMenu.ReadOptions(Switcher.Right);
                    break;

                case Key.Enter:
                    currentMenu.ReadOptions(Switcher.None);
                    break;

                default:
                    break;
            }
        }
        private void StartScreen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _menuPages[_current].MenuControl.ReadOptions(Switcher.None);
        }
        private void StartScreen_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
