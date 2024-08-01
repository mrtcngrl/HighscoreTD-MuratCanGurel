using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.UI.Views
{
    public class ScorePopup : MonoBehaviour
    {
        [SerializeField] private GameObject _scorePopup;
        [SerializeField] private Button _retryButton;
        [SerializeField] private TextMeshProUGUI _scoreText;

        private void Start()
        {
            _retryButton.onClick.AddListener(OnRetryClicked);
            _scorePopup.transform.localScale = Vector3.zero;
        }

        public void ShowPopup()
        {
            gameObject.SetActive(true);
            _scorePopup.transform.DOScale(1, 0.3f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                _retryButton.interactable = true;
            });
        }

        private void HidePopup()
        {
            _scorePopup.transform.DOScale(0, 0.25f).SetEase(Ease.InOutBack).OnComplete(()=>
            {
                gameObject.SetActive(false);
                _scorePopup.transform.localScale = Vector3.zero;
            });
        }

        private void OnRetryClicked()
        {
            HidePopup();
            _retryButton.interactable = false;
        }
    }
}