using System;
using System.Linq;
using Game.Pool;
using Scripts.Game.Components.Enemy;
using Scripts.Game.Components.Enemy.Interface;
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
        [SerializeField] protected Transform Muzzle;
        protected float Damage;
        protected float Range;
        protected float Cooldown;
        protected bool IsActive;
        protected IDisposable SearchRoutine;
        protected IDisposable FireRoutine;
        protected IHittable ClosestTarget;
        protected Spawner Spawner;
        protected bool HasTimer;
        private bool _isPlaced;

        protected virtual void Initialize()
        {
            Spawner = Spawner.Instance;
            IsActive = true;
            Damage = _properties.Damage;
            Range = _properties.Range;
            Cooldown = _properties.Cooldown;
        }

        protected virtual void Activate()
        {
            HasTimer = false;
            Initialize();
            SearchRoutine?.Dispose();
            SearchRoutine = Observable.Timer(TimeSpan.FromSeconds(.1f)).Repeat().Subscribe(_=>CheckArea());
        }

        protected virtual void CheckArea()
        {
            var Colliders = Physics.OverlapSphere(transform.position, Range, GameConstants.Enemy).ToList();
            float closestDistanceSqr = Mathf.Infinity;
            Collider targetCollider = null;
            ClosestTarget = null;
            foreach (var collider in Colliders)
            {
                var candidateDistance = (collider.transform.position - Position).sqrMagnitude;
                if (candidateDistance < closestDistanceSqr)
                {
                    targetCollider = collider;
                    closestDistanceSqr = candidateDistance;
                }
                ClosestTarget = targetCollider.attachedRigidbody?.GetComponent<IHittable>();
            }

            TryToFire();
            
        }

        protected virtual void TryToFire()
        {
            if (ClosestTarget != null && !HasTimer)
            {
                Fire();
                FireRoutine?.Dispose();
                HasTimer = true;
                FireRoutine = Observable.Timer(TimeSpan.FromSeconds(Cooldown)).Subscribe(_=>HasTimer = false);
            }
        }
        protected abstract void Fire();
        
        public Vector3 Position => transform.position;
        public bool Available => !_isPlaced;
        
        public void OnHold(Vector3 mouseWorldPos)
        {
            var candidatePosition = mouseWorldPos + Vector3.forward;
            candidatePosition = candidatePosition.CopyWithY(1.5f);
            transform.position = candidatePosition;
        }

        public void OnRelease(Vector3 hitPoint, out bool placed)
        {
            _isPlaced = placed = false;
            var slot = SlotController.Instance.GetNearestAvailableSlot(hitPoint);
            if(slot == null) return;
            _isPlaced = placed = true;
            slot.PlaceTurret(this);
            Activate();
            CheckArea();
            GameConstants.OnFirstTurretPlaced?.Invoke();
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, Range);
        }
    }
}