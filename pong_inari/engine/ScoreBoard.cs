using System;
using System.Windows.Controls;

namespace pong_inari.engine
{
    public class ScoreBoard
    {
        public event EventHandler<int> OnChange;
        private int _currentScore;
        public Label View { get; set; }
        public int CurrentScores 
        { 
            get { return _currentScore; }
            set { _currentScore = value; OnChange.Invoke(this, value); }
        }
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
