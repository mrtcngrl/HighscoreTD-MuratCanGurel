using System;
using Scripts.Game.Components.TurretSystem.Scriptable;
using Scripts.Game.Components.TurretSystem.TurretSlot;
using Scripts.Game.Controllers;
using Scripts.Helpers;
using UniRx;
using UnityEngine;

namespace Scripts.Game.Components.TurretSystem.Turrets
{
    public abstract class TurretBase : MonoBehaviour, ISelectable
    {
        [SerializeField] protected TurretProperties _properties;
        protected float Damage;
        protected float Range;
        protected float Cooldown;
        protected bool IsActive;
        private IDisposable SearchRoutine;

        protected virtual void Initialize()
        {
            Damage = _properties.Damage;
            Range = _properties.Range;
            Cooldown = _properties.Cooldown;
        }

        protected virtual void Activate()
        {
            SearchRoutine?.Dispose();
            SearchRoutine = Observable.EveryUpdate().Subscribe(_=>CheckArea());
        }
        protected abstract void CheckArea();
        protected abstract void Fire();

       
        
        public virtual void OnPlace()
        {
            IsActive = true;
            Activate();
        }


        public Vector3 Position => transform.position;
        public bool Available => !IsActive;
        
        public void OnHold(Vector3 mouseWorldPos)
        {
            var candidatePosition = mouseWorldPos + Vector3.forward;
            candidatePosition = candidatePosition.CopyWithY(1.5f);
            transform.position = candidatePosition;
        }

        public void OnRelease(Vector3 hitPoint, out bool placed)
        {
            placed = false;
            var slot = SlotController.Instance.GetNearestAvailableSlot(hitPoint);
            if(slot == null) return;
            placed = true;
            slot.PlaceTurret(this);
            Activate();
        }
    }
}