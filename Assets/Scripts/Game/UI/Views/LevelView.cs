using System;
using DG.Tweening;
using Scripts.Game.Controllers;
using Scripts.Game.UI.Popup;
using Scripts.User;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Game.UI.Views
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private ScorePopup _scorePopup;
        [SerializeField] private RecoverPopup _recoverPopup;
        [SerializeField] private Image _boosterProgressImage;
        [SerializeField] private Button _boosterButton;
        private Tween _fillerTween;
        private float _fillAmount;
        private Sequence _pulseAnim;
        private IDisposable _boosterTimer;
        private UserProgressData _userProgressData;
        [Inject]
        private void OnInject(UserProgressDataManager userProgressDataManager)
        {
            _userProgressData = userProgressDataManager.Progress;
            GameConstants.CoinAmountChanged += OnCoinAmountChanged;
            GameConstants.ScoreChanged += OnScoreChanged;
            GameConstants.OnSessionEnd += OnSessionEnd;
            GameConstants.OnEnemyDie += FillBooster;
            _pulseAnim = DOTween.Sequence()
                .Append(_boosterButton.transform.DOScale(1.2f, 0.2f))
                .Append(_boosterButton.transform.DOScale(1f, 0.2f)).SetAutoKill(false);
            _pulseAnim.Pause();
            _boosterProgressImage.fillAmount = 0;
            _boosterButton.onClick.AddListener(OnBoosterButtonClicked);
        }

        private void OnDestroy()
        {
            GameConstants.CoinAmountChanged -= OnCoinAmountChanged;
            GameConstants.ScoreChanged -= OnScoreChanged;
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

        private void FillBooster()
        {
            if(_fillAmount >= 1f || GameController.BoosterActive.Value) return;
            _fillAmount = Mathf.Clamp01(_fillAmount + GameConstants.BoosterIncreasePercent);
            SetFillerValue(_fillAmount);
        }

        private void SetBoosterAvailable()
        {
            _boosterButton.interactable = true;
            _pulseAnim?.Restart();
        }

        private void SetFillerValue(float fillAmount, float duration = .2f)
        {
            _fillerTween?.Kill();
            float value = _boosterProgressImage.fillAmount;
            _fillerTween = DOTween.To(() => value, x => value = x, fillAmount, duration)
                .OnUpdate(() =>
                {
                    _boosterProgressImage.fillAmount = value;
                }).OnComplete(() =>
                {
                    if (_fillAmount >= 1)
                    {
                        SetBoosterAvailable();
                    }
                });
        }
        private void OnBoosterButtonClicked()
        {
            _fillAmount = 0;
            GameController.BoosterActive.Value = true;
            _boosterButton.interactable = false;
            _pulseAnim?.Pause();
            _boosterTimer?.Dispose();
            SetFillerValue(_fillAmount,GameConstants.BoosterDuration);
            _boosterTimer = Observable.Timer(TimeSpan.FromSeconds(GameConstants.BoosterDuration))
                .Subscribe(_ => OnBoosterEnd());
        }
        private void OnBoosterEnd()
        {
            GameController.BoosterActive.Value = false;
        }

        private void OnSessionEnd()
        {
            _fillAmount = 0;
            _boosterProgressImage.fillAmount = 0;
            _scorePopup.ShowPopup();
            _boosterTimer?.Dispose();
            _userProgressData.HasValue = false;
        }
    }
}