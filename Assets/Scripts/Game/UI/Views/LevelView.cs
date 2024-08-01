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
        [SerializeField] private ScorePopup _scorePopup;
        [Inject]
        private void OnInject()
        {
            GameConstants.CoinAmountChanged += OnCoinAmountChanged;
            GameConstants.ScoreChaged += OnScoreChanged;
            GameConstants.OnSessionEnd += OnSessionEnd;
        }

        private void OnDestroy()
        {
            GameConstants.CoinAmountChanged -= OnCoinAmountChanged;
            GameConstants.ScoreChaged -= OnScoreChanged;
            GameConstants.OnSessionEnd -= OnSessionEnd;
        }

        private void OnCoinAmountChanged(int amount)
        {
            _coinText.text = amount.ToString();
        }

        private void OnScoreChanged(int score)
        {
            _scoreText.text = score.ToString();
        }

        private void OnSessionEnd()
        {
            _scorePopup.ShowPopup();
        }
    }
}