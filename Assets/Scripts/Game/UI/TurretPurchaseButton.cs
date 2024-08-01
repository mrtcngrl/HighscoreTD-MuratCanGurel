using Game.Pool;
using Scripts.Game.Components.TurretSystem.Scriptable;
using Scripts.Game.Components.TurretSystem.Turrets;
using Scripts.Game.Controllers;
using Scripts.User;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Game.UI
{
    public class TurretPurchaseButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private int _price;
        [SerializeField] private TurretProperties _turretProperties;
        [SerializeField] private TextMeshProUGUI _priceText;
        private SelectionController _selectionController;
        private UserProgressData _userProgressData;
        [Inject]
        private void OnInject(Spawner spawner, SelectionController selectionController, UserProgressData userProgressData)
        {
            _selectionController = selectionController;
            _priceText.text = _price.ToString();
            _button.onClick.AddListener(Create);
            _userProgressData = userProgressData;
        }

        private void Create()
        {
            if(_price > _userProgressData.CoinAmount ||!GameController.Instance.CanSpawnNewTurret()) return;
            TurretBase turret =Instantiate(_turretProperties.Prefab, Vector3.back * 10, Quaternion.identity).GetComponent<TurretBase>();
            GameController.Instance.AddTurret(turret);
             _selectionController.SetSelectable(turret);
             GameConstants.CoinEarned?.Invoke(-_price);
        }
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}