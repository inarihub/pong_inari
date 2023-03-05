using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace pong_inari.uicontrols
{
    public class MenuControl
    {
        int current;
        public MenuSelector Selector { get; set; }
        public List<GameOption> OptionsList { get; set; }
        public int CurrentOption
        {
            get 
            { 
                return current; 
            }
            set
            {
                if (current != value)
                {
                    SelectedOptionChanged.Invoke(this, value);
                }
            }
        }
        public Grid MenuGrid { get; set; }

        public event EventHandler<int> SelectedOptionChanged;
        // remove unused
        public MenuControl(Grid grid)
        {
            MenuGrid = grid;
            Selector = new MenuSelector();
            OptionsList = new List<GameOption>();
            CurrentOption = 0;
            SelectedOptionChanged += OnChangedHandler;
        }
        public void SetMouseFeedback()
        {
            OptionsList.ForEach(x => { x.MenuLabel.MouseMove += MenuLabel_MouseMove; });
        }
        private void MenuLabel_MouseMove(object sender, MouseEventArgs e)
        {
            int newIndex = OptionsList.FindIndex(x => x.MenuLabel == (Label)sender);
            ChangeSelection(current, newIndex);
        }
        private void OnChangedHandler(object? sender, int newIndex)
        {
            if (sender is null) { return; }
            var oldIndex = ((MenuControl)sender).current;
            if (newIndex > OptionsList.Count - 1)
            {
                newIndex = 0;
            }
            else if (newIndex < 0)
            {
                newIndex = OptionsList.Count - 1;
            }

            ChangeSelection(oldIndex, newIndex);
        }
        private void ChangeSelection(int oldIndex, int newIndex)
        {
            current = newIndex;
            var newOption = OptionsList[newIndex];
            var oldOption = OptionsList[oldIndex];
            var newLabelRow = newOption.MenuLabel.GetValue(Grid.RowProperty);
            Selector.Item.SetValue(Grid.RowProperty, newLabelRow);
            oldOption.IsSelected = false;
            newOption.IsSelected = true;
            ChangeSelectedColors(oldOption, newOption);
        }
        private void ChangeSelectedColors(GameOption oldOption, GameOption newOption)
        {
            oldOption.MenuLabel.Foreground = App.Current.Resources["Unselected"] as SolidColorBrush;
            if (oldOption.DependencyObject is Label)
            {
                ((Label)oldOption.DependencyObject).Foreground = App.Current.Resources["Unselected"] as SolidColorBrush;
            }
            newOption.MenuLabel.Foreground = App.Current.Resources["Selected"] as SolidColorBrush;
            if (newOption.DependencyObject is Label)
            {
                ((Label)newOption.DependencyObject).Foreground = App.Current.Resources["Selected"] as SolidColorBrush;
            }
        }
        public void MoveDown()
        {
            CurrentOption++;
        }
        public void MoveUp()
        {
            CurrentOption--;
        }
        public void SetSelection()
        {
            if (!MenuGrid.Children.Contains(Selector.Item))
            {
                MenuGrid.Children.Add(Selector.Item);
            }
            Selector.Item.SetValue(Grid.ColumnProperty, 1);
            ChangeSelection(current, 0);
            Selector.Item.SetVisible();
        }
        public void ReadOptions(Switcher dir)
        {
            try
            {
                if (OptionsList[current].IsSwitchable)
                {
                    OptionsList[current].OptionActivation.Invoke(dir);
                }
                else
                {
                    OptionsList[current].OptionActivation.Invoke(Switcher.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                App.Current.Shutdown();
            }
        }
    }
}
