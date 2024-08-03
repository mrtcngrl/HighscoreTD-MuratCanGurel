using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Game.Components.TurretSystem.Scriptable;
using Scripts.Game.Components.TurretSystem.Turrets;
using Scripts.Game.Components.TurretSystem.TurretSlot;
using Scripts.Helpers;
using Scripts.User;
using UniRx;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private List<TurretProperties> _turretProperties = new();
        private List<TurretBase> _turrets = new();
        private SlotController _slotController;
        public static GameController Instance;
        private UserProgressDataManager _userProgressDataManager;
        public static ReactiveProperty<bool> BoosterActive = new();
        public int Score => _userProgressDataManager.Progress.Score;
        [Inject]
        private void OnInject(UserProgressDataManager userProgressDataManager)
        {
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
            _userProgressDataManager = userProgressDataManager;
            GameConstants.OnRetry += Initialize;
            BoosterActive.Value = false;
        }
        private void Start()
        {
            _slotController = SlotController.Instance;
            Initialize();
        }

        private void Initialize()
        {
            _userProgressDataManager.SetCoinAmount(GameConstants.StartCoinAmount);
            _userProgressDataManager.SetScore(0);
        }
        
        private void AddTurret(TurretBase turret)
        {
            _turrets.Add(turret);
        }

        public TurretBase CreateTurret(GameObject turretPrefab)
        {
            TurretBase turret =Instantiate(turretPrefab).GetComponent<TurretBase>();
            AddTurret(turret);
            return turret;
        }
        public TurretBase CreateTurret(GameObject turretPrefab, Vector3 position)
        {
            TurretBase turret =Instantiate(turretPrefab, position, Quaternion.identity).GetComponent<TurretBase>();
            AddTurret(turret);
            return turret;
        }

        public TurretBase CreateTurret(int id)
        {
            GameObject prefab = _turretProperties.FirstOrDefault(p => p.ID == id)?.Prefab;
            return CreateTurret(prefab);
        }
        public bool CanSpawnNewTurret()
        {
            return _turrets.TrueForAll(t => !t.Available) && _slotController.AnyEmptySlot();
        }
    }
}