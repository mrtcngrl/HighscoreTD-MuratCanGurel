using System;
using Scripts.Game.Components.TurretSystem.Scriptable;
using Scripts.Game.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.UI
{
    public class TurretPurchaseButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TurretProperties _turretProperties;
        [SerializeField] private TextMeshProUGUI _priceText;

        private SpawnController _spawnController;
        private GameObject testObj;
        private void Start()
        {
            //_button.onClick.AddListener(()=> _spawnController.SpawnTurretByID(_turretProperties.ID));
            _button.onClick.AddListener(Create);
        }

        private void Create()
        {
            Instantiate(_turretProperties.Prefab, Vector3.zero, Quaternion.identity);
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