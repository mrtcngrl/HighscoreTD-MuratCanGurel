using System;
using Scripts.Game.Components.TurretSystem.Projectiles;
using Scripts.Helpers;
using UniRx;
using UnityEngine;

namespace Scripts.Game.Components.TurretSystem.Turrets
{
    public class Mine : TurretBase
    {
        private Projectile _currentMine;
        
        protected override void Activate()
        {
            base.Initialize();
            SearchRoutine?.Dispose();
            SearchRoutine = Observable.Timer(TimeSpan.FromSeconds(Cooldown)).Repeat().Subscribe(_=>CheckArea());
        }

        public override void OnBoosterValueChange(bool isBoost)
        {
            
            Cooldown = isBoost ? Cooldown / 2f : Properties.Cooldown;
            SearchRoutine?.Dispose();
            SearchRoutine = Observable.Timer(TimeSpan.FromSeconds(Cooldown)).Repeat().Subscribe(_=>CheckArea());
        }

        protected override void CheckArea()
        {
            if(_currentMine == null || !_currentMine.gameObject.activeInHierarchy || _currentMine.ProjectileType != ProjectileType.Mine)
                Fire();
        }

        protected override void Fire()
        {
            _currentMine = Spawner.SpawnProjectile(Muzzle.transform.position);
            _currentMine.Launch(ProjectileType.Mine,transform.position.CopyWithX(0),Damage);
        }
    }
}