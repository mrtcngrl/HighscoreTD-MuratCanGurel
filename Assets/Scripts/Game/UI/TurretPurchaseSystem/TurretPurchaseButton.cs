using System;
using Game.Pool;
using Scripts.Game.Components.TurretSystem.Scriptable;
using Scripts.Game.Components.TurretSystem.Turrets;
using Scripts.Game.Controllers;
using Scripts.User;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Game.UI.TurretPurchaseSystem
{
    public class TurretPurchaseButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private int _price;
        [SerializeField] private TurretProperties _turretProperties;
        [SerializeField] private TextMeshProUGUI _priceText;
        private SelectionController _selectionController;
        private UserProgressDataManager _userProgressDataManager;
        public TurretProperties TurretProperties => _turretProperties;
        private Action<TurretPurchaseButton> _callback;
        [Inject]
        private void OnInject(Spawner spawner, SelectionController selectionController, UserProgressDataManager userProgressDataManager)
        {
            _selectionController = selectionController;
            _userProgressDataManager = userProgressDataManager;
        }

        public void Initialize(int price, Action<TurretPurchaseButton> callback)
        {
            SetPrice(price);
            _button.onClick.AddListener(Create);
            _callback = callback;
        }

        private void Create()
        {
            Debug.LogError("Clicked");
            if(_price > _userProgressDataManager.CoinAmount ||!GameController.Instance.CanSpawnNewTurret()) return;
            var turret = GameController.Instance.CreateTurret(_turretProperties.Prefab, Vector3.back * 10);
             _selectionController.SetSelectable(turret);
             GameConstants.CoinEarned?.Invoke(-_price);
             _callback?.Invoke(this);
        }
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void SetPrice(int price)
        {
            _price = price;
            _priceText.text = _price.ToString();
        }
    }
}