using System;
using System.Collections.Generic;
using Scripts.Game.Components.TurretSystem.Turrets;
using Scripts.Game.Components.TurretSystem.TurretSlot;
using Scripts.Helpers;
using Scripts.User;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        private List<TurretBase> _turrets = new();

        private SlotController _slotController;
        public static GameController Instance;
        private UserProgressData _userProgressData;
        [Inject]
        private void OnInject(UserProgressData userProgressData)
        {
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
            _userProgressData = userProgressData;
            GameConstants.OnRetry += Initialize;
        }
        private void Start()
        {
            _slotController = SlotController.Instance;
            Initialize();
        }

        private void Initialize()
        {
            _userProgressData.SetCoinAmount(GameConstants.StartCoinAmount);
            _userProgressData.SetScore(0);
        }
        
        public void AddTurret(TurretBase turret)
        {
            _turrets.Add(turret);
        }

        public bool CanSpawnNewTurret()
        {
            return _turrets.TrueForAll(t => !t.Available) && _slotController.AnyEmptySlot();
        }
    }
}