using Scripts.Game;
using UnityEngine;
using Zenject;

namespace Scripts.User
{
    public class UserProgressData
    {
        public int CoinAmount => coinAmount;
        private int coinAmount;
        private int score;
        public UserProgressData()
        {
            GameConstants.CoinEarned += OnCoinAmountChanged;
            GameConstants.ScoreEarned += OnScoreChanged;
        }
        
        ~UserProgressData()
        {
            GameConstants.CoinEarned -= OnCoinAmountChanged;
            GameConstants.ScoreEarned += OnScoreChanged;
        }
        private void OnCoinAmountChanged(int amount)
        {
            coinAmount += amount;
            GameConstants.CoinAmountChanged?.Invoke(coinAmount);
        }
        private void OnScoreChanged(int amount)
        {
            score += amount;
            GameConstants.ScoreChaged?.Invoke(score);
        }

        public void SetCoinAmount(int amount)
        {
            coinAmount = amount;
            GameConstants.CoinAmountChanged?.Invoke(coinAmount);
        }
        public void SetScore(int amount)
        {
            score = amount;
            GameConstants.ScoreChaged?.Invoke(score);
        }
    }
}