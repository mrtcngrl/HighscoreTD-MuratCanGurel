using System;
using System.Linq;
using Scripts.Game.Components.Enemy.Interface;
using Scripts.Game.Components.TurretSystem.Projectiles;
using Scripts.Helpers;
using UnityEngine;

namespace Scripts.Game.Components.TurretSystem.Turrets
{
    public class Mortar : TurretBase
    {
        [SerializeField] private Transform _head;
        [SerializeField] private float _anglePerSecond;
        [SerializeField, Range(0,1)] private float _deadRangePercentage;
        private float _deadRange;

        protected override void Initialize()
        {
            base.Initialize();
            _deadRange = Range * _deadRangePercentage;
        }

        protected override void CheckArea()
        {
            Debug.LogError("cheking-1");
            var Colliders = Physics.OverlapSphere(transform.position, Range, GameConstants.Enemy).ToList();
            float closestDistanceSqr = Mathf.Infinity;
            Collider targetCollider = null;
            ClosestTarget = null;
            foreach (var collider in Colliders)
            {
                var candidateDistance = (collider.transform.position - Position).sqrMagnitude;
                if (candidateDistance < closestDistanceSqr && candidateDistance >= _deadRange)
                {
                    targetCollider = collider;
                    closestDistanceSqr = candidateDistance;
                }
                ClosestTarget = targetCollider.attachedRigidbody?.GetComponent<IHittable>();
            }

            if (ClosestTarget != null)
                Fire();
        }

        private void Update()
        {
            if (ClosestTarget != null)
            {
                Vector3 direction = (ClosestTarget.Transform.position.CopyWithY(1.5f) - Position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                _head.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                    _anglePerSecond);
            }
        }

        protected override void Fire()
        {
            ;
            if (ClosestTarget != null)
            {
                var bullet = Spawner.SpawnProjectile(Muzzle.position);
                bullet.Launch(ProjectileType.Mortar, ClosestTarget.Transform.position, Damage);
            }
        }

        protected override void Activate()
        {
            base.Activate();
        }
    }
}