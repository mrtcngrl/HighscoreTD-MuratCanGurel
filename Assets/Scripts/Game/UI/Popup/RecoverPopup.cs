using Game.UI.Popup;
using Scripts.Game.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.UI.Views
{
    public class RecoverPopup : PopupBase
    {
        [SerializeField] private Button _noButton;
        
        protected override void Awake()
        {
            base.Awake();
            _noButton.onClick.AddListener(NoButtonClicked);
            GameConstants.OnDataLoad += ShowPopup;
        }

        public override void ShowPopup()
        {
            _noButton.interactable = true;
            base.ShowPopup();
        }

        protected override void OnButtonClicked()
        {
            base.OnButtonClicked();
            DatabaseController.Instance.LoadRecoveredData();
        }

        private void NoButtonClicked()
        {
            _noButton.interactable = false;
            DatabaseController.Instance.ClearData();
            HidePopup();
        }
    }
}