using Game.Pool;
using Scripts.Game.Components.TurretSystem.Scriptable;
using Scripts.Game.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Scripts.Game.UI
{
    public class TurretPurchaseButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TurretProperties _turretProperties;
        [SerializeField] private TextMeshProUGUI _priceText;
        private Spawner _spawner;
        private SelectionController _selectionController;
        
        [Inject]
        private void OnInject(Spawner spawner, SelectionController selectionController)
        {
            _spawner = spawner;
            _selectionController = selectionController;
            Debug.LogError("Zenject test");
            //_button.onClick.AddListener(()=> _spawnController.SpawnTurretByID(_turretProperties.ID));
            _button.onClick.AddListener(Create);
        }

        private void Create()
        {
             ISelectable selectedObject = Instantiate(_turretProperties.Prefab, Vector3.zero, Quaternion.identity).GetComponent<ISelectable>();
             _selectionController.SetSelectable(selectedObject);
        }
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        //todo set data
        public void Initialize()
        {
            
        }

    }
}