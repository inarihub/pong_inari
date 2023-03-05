using pong_inari.MainMenuPages;
using pong_inari.uicontrols;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pong_inari
{
    public partial class MainWindow : Window
    {
        // to IGameMenu
        public Dictionary<string, IGameMenu> menuPages;
        public string current;
        public MainWindow()
        {
            InitializeComponent();
            menuPages = new Dictionary<string, IGameMenu>
            {
                { "StartMenu", new StartMenuView() },
                { "OptionsMenu", new OptionsMenuView() }
            };
            current = "StartMenu";
            this.ContentRendered += PageRenderedHandler;
        }
        private void PageRenderedHandler(object? sender, EventArgs e)
        {
            ChangeMenu(current);
        }
        public void ChangeMenu(string menu)
        {
            menuPages[current].MenuControl.SetSelection();
            current = menu;
            Content = menuPages[menu];
        }
        public void KeyDownHandler(object sender, KeyEventArgs e)
        {
            var currentMenu = menuPages[current].MenuControl;

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
            menuPages[current].MenuControl.ReadOptions(Switcher.None);
        }
    }
}
