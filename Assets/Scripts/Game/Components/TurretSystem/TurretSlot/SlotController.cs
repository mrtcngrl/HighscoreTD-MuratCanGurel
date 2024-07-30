using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Game.Components.TurretSystem.TurretSlot
{
    [ExecuteInEditMode]
    public class SlotController : MonoBehaviour
    {
        [SerializeField] private List<Vector3> _slotPositions = new();
        [SerializeField] private int _slotCount;
        
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