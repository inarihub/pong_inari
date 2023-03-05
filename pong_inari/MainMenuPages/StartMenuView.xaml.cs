using pong_inari.engine;
using pong_inari.uicontrols;
using System;
using System.Windows;
using System.Windows.Controls;

namespace pong_inari.MainMenuPages
{
    /// <summary>
    /// Interaction logic for StartMenuView.xaml
    /// </summary>
    public partial class StartMenuView : Page, IGameMenu
    {
        public Grid MenuGrid { get; set; }
        public MenuControl MenuControl { get; set; }

        private GameWindow gameWindow;
        public StartMenuView()
        {
            InitializeComponent();
            gameWindow = new GameWindow();
            MenuGrid = StartMenuGrid;
            MenuControl = new(MenuGrid);
            InitializeOptions();
        }
        private void InitializeOptions()
        {
            MenuControl.OptionsList.Add(new GameOption(Start, StartGame));
            MenuControl.OptionsList.Add(new GameOption(Options, EnterOptions));
            MenuControl.OptionsList.Add(new GameOption(Exit, ExitGame));
            MenuControl.SetMouseFeedback();
        }
        private void StartGame(Switcher dir)
        {
            App.Current.MainWindow.Hide();
            gameWindow.Show();
        }
        private void EnterOptions(Switcher dir)
        {
            ((MainWindow)App.Current.MainWindow).ChangeMenu("OptionsMenu");
        }
        private void ExitGame(Switcher dir)
        {
            App.Current.Shutdown();
        }
    }
}
