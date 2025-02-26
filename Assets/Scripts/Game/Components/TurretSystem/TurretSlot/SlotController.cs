using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Game.Controllers;
using Scripts.Helpers;
using Scripts.User;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Components.TurretSystem.TurretSlot
{
    [ExecuteInEditMode]
    public class SlotController : MonoBehaviour
    {
        [SerializeField] private List<Vector3> _slotPositions = new();
        [SerializeField] private int _slotCount;
        public static SlotController Instance;
        private List<Slot> _slots = new();
        private UserProgressData _progress;
        [Inject]
        private void OnInject(UserProgressDataManager userProgressDataManager)
        {
            _progress = userProgressDataManager.Progress;
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
            Initialize();
            GameConstants.OnSessionEnd += OnSessionEnd;
            GameConstants.OnRetry += Initialize;
            GameConstants.OnDataRecover += OnDataRecover;
        }
        private void Initialize()
        {
            _slots.Clear();
            for (int i = 0; i < _slotCount; i++)
            {
                _slots.Add(new Slot(i+1, true, _slotPositions[i]));
            }
        }
        public Slot GetNearestAvailableSlot(Vector3 hitPoint)
        {
            hitPoint = hitPoint.CopyWithY(0);
            Slot bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            
            foreach (var slot in _slots)
            {
                if(!slot.Available) continue;
                Vector3 directionToSlot = slot.Position.CopyWithY(0) - hitPoint;
                float dSqrToTarget = directionToSlot.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = slot;
                }
            }
            return closestDistanceSqr <= GameConstants.MaxDistanceToPlace ? bestTarget : null; 
        }

        public bool AnyEmptySlot()
        {
            return _slots.Any(s => s.Available);
        }
        
        
        private void OnSessionEnd()
        {
            foreach (var slot in _slots.Where(slot => !slot.Available))
            {
                slot.RemoveTurret();
            }
        }
        public void SaveSlotData(Slot slot)
        {
            _progress.SlotData.SlotInfos.Add(new SlotInfo()
            {
                HasTurret = true,
                ID = slot.ID,
                TurretID = slot.Turret.ID
            });
        }

        private void OnDataRecover(UserProgressData progress)
        {
            foreach (var slotDataSlotInfo in progress.SlotData.SlotInfos)
            {
                var slot = _slots[slotDataSlotInfo.ID - 1];
                var turret = GameController.Instance.CreateTurret(slotDataSlotInfo.TurretID);
                slot.PlaceTurret(turret);
                turret.LoadPlace();
            }
        }
#if UNITY_EDITOR
        [SerializeField] private float radius = 5f;
        [SerializeField] private float padding = 1f;
        [Button]
        private void SetPositions()
        {
            int half = _slotCount / 2;
            for (int i = 0; i < _slotCount; i++)
            {
                _slotPositions[i] = transform.position + new Vector3(i >=half ? -radius : radius, 0, padding * (i % 5f));
            }
        }
        private void OnDrawGizmos()
        {
            foreach (var slotPosition in _slotPositions)
            {
                Gizmos.DrawSphere(slotPosition, .2f);
            }
        }
#endif
    }
}