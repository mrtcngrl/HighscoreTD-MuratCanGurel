using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scripts.Game.UI.Views
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _scoreText;

        [Inject]
        private void OnInject()
        {
            GameConstants.CoinAmountChanged += OnCoinAmountChanged;
            GameConstants.ScoreChaged += OnScoreChanged;
        }

        private void OnDestroy()
        {
            GameConstants.CoinAmountChanged -= OnCoinAmountChanged;
            GameConstants.ScoreChaged -= OnScoreChanged;
        }

        private void OnCoinAmountChanged(int amount)
        {
            _coinText.text = amount.ToString();
        }

        private void OnScoreChanged(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}