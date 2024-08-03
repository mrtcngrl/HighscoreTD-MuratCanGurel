using System.Linq;
using Scripts.Game;
using Scripts.Game.UI.TurretPurchaseSystem;
using UnityEngine;
using Zenject;

namespace Scripts.User
{
    public class UserProgressDataManager
    {
        public int CoinAmount => _coinAmount;
        private int _coinAmount;
        private int score;
        public UserProgressData Progress;
        
        public UserProgressDataManager()
        {
            GameConstants.CoinEarned += OnCoinAmountChanged;
            GameConstants.ScoreEarned += OnScoreChanged;
            Progress = new UserProgressData
            {
                HasValue = false
            };
            for (int i = 0; i < 3; i++)
            {
                Progress.PurchaseProgressData.PurchaseProgresses.Add(new PurchaseProgress
                {
                    ID = i+1,
                    PurchaseStep = 0
                });
            }
        }
        
        ~UserProgressDataManager()
        {
            GameConstants.CoinEarned -= OnCoinAmountChanged;
            GameConstants.ScoreEarned += OnScoreChanged;
        }
        private void OnCoinAmountChanged(int amount)
        {
            _coinAmount += amount;
            Progress.CoinAmount = _coinAmount;
            GameConstants.CoinAmountChanged?.Invoke(_coinAmount);
        }
        private void OnScoreChanged(int amount)
        {
            score += amount;
            Progress.Score = score;
            GameConstants.ScoreChanged?.Invoke(score);
        }

        public void SetCoinAmount(int amount)
        {
            _coinAmount = amount;
            Progress.CoinAmount = _coinAmount;
            GameConstants.CoinAmountChanged?.Invoke(_coinAmount);
        }
        public void SetScore(int amount)
        {
            score = amount;
            Progress.Score = score;
            GameConstants.ScoreChanged?.Invoke(score);
        }

        public PurchaseProgress GetPurchaseDataById(int id)
        {
            return Progress.PurchaseProgressData.PurchaseProgresses.FirstOrDefault(p => p.ID == id);
        }
    }
}