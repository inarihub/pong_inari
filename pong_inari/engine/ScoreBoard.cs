using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace pong_inari.engine
{
    public class ScoreBoard
    {
        public Label View { get; set; }
        public int CurrentScores 
        { 
            get { return _currentScore; }
            set { _currentScore = value; OnChange.Invoke(this, value); }
        }
        private int _currentScore;
        public event EventHandler<int> OnChange;
        public ScoreBoard(Label scoreBoard)
        {
            View = scoreBoard;
            _currentScore = 0;
            OnChange += ScoreChangedHandler;
        }

        private void ScoreChangedHandler(object? sender, int value)
        {
            UpdateScores(value);
        }
        public void ResetScores()
        {
            CurrentScores = 0;
        }
        public void UpdateScores(int value)
        {
            View.Dispatcher.BeginInvoke(() =>
            {
                View.Content = CurrentScores.ToString("000000000000000");
            }); 
        }
        public static ScoreBoard operator +(ScoreBoard playerScore, int plusScore)
        {
            playerScore.CurrentScores += plusScore;
            return playerScore;
        }
    }
}
