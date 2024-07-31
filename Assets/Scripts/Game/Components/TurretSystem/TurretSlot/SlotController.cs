using System;
using System.Collections.Generic;
using Scripts.Helpers;
using Sirenix.OdinInspector;
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


        [Inject]
        private void OnInject()
        {
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
            Initialize();
        }
        private void Initialize()
        {
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