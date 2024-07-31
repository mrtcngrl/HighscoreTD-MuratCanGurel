using System;
using System.Collections.Generic;
using Scripts.Game.Components;
using Scripts.Game.Components.Enemy;
using Scripts.Game.Components.TurretSystem.Projectiles;
using Scripts.Helpers;
using UnityEngine;
using Zenject;

namespace Game.Pool
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private Transform _enemySpawnTransform;
        private ObjectPool<Projectile> _bulletPool;
        private ObjectPool<Stickman> _stickmanPool;
        private Vector3 stickmanTargetPoint;
        public static Spawner Instance;
        [Inject]
        private void OnInject()
        { 
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else 
                Instance = this;
            _bulletPool = new ObjectPool<Projectile>(_bulletPrefab);
            _stickmanPool = new ObjectPool<Stickman>(_enemyPrefab);
        }

        private void Start()
        {
            stickmanTargetPoint = Castle.Instance.CastleDoor;
        }
        
        
        
        public Projectile SpawnProjectile(Vector3 position)
        {
            return _bulletPool.Pull(position);
        }

        public void SpawnStickman()
        {
            Stickman stickman = _stickmanPool.Pull(_enemySpawnTransform.position);
            stickman.Initialize(stickmanTargetPoint,100, 5);
        }
    }
}