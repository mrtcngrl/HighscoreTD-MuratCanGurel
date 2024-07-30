using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game.Components.TurretSystem.TurretSlot
{
    [ExecuteInEditMode]
    public class SlotController : MonoBehaviour
    {
        [SerializeField] private List<Vector3> _slotPositions = new List<Vector3>(10);
        
        
        private void OnDrawGizmos()
        {
            int count = _slotPositions.Count;
            for (int i = 0; i < count; i++)
            {
                //Gizmos.DrawSphere(transform.position);
            }
        }
    }
}