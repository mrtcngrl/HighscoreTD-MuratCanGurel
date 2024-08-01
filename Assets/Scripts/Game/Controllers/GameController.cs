using System;
using System.Collections.Generic;
using Scripts.Game.Components.TurretSystem.Turrets;
using Scripts.Game.Components.TurretSystem.TurretSlot;
using Scripts.Helpers;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Controllers
{
    public class GameController : MonoBehaviour
    {
        private List<TurretBase> _turrets = new();

        private SlotController _slotController;
        public static GameController Instance;
        [Inject]
        private void OnInject()
        {
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
        }
        private void Start()
        {
            _slotController = SlotController.Instance;
            GameConstants.CoinEarned?.Invoke(50);
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