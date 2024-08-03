using Game.UI.Popup;
using Scripts.Game.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.UI.Popup
{
    public class ScorePopup : PopupBase
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        public override void ShowPopup()
        {
            base.ShowPopup();
            _scoreText.text = GameController.Instance.Score.ToString();
        }

        protected override void OnButtonClicked()
        {
            base.HidePopup();
            GameConstants.OnRetry?.Invoke();
        }
    }
}