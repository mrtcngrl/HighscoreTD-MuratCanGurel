using System;
using Scripts.Game.Components.TurretSystem.Projectiles;
using UnityEngine;

namespace Scripts.Game.Components.TurretSystem.Turrets
{
    public class Turret: TurretBase
    {
        [SerializeField] private Transform _head;
        [SerializeField] private float _anglePerSecond;
        protected override void Activate()
        {
           base.Activate();
        }

        protected override void CheckArea()
        {
            base.CheckArea();
            if (ClosestTarget != null)
                Fire();
        }

        private void Update()
        {
            if (ClosestTarget != null)
            {
                Vector3 direction = (ClosestTarget.Transform.position - Position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                _head.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                    _anglePerSecond);
            }
        }

        protected override void Fire()
        {
            if (ClosestTarget != null)
            {
                var bullet = Spawner.SpawnProjectile(Muzzle.position);
                bullet.Launch(ProjectileType.Bullet, ClosestTarget.Transform.position, Damage);
            }
        }
        
    }
}