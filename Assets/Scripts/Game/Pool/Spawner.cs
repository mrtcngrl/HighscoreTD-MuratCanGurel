using System;
using System.Collections.Generic;
using Scripts.Game.Components;
using Scripts.Game.Components.Enemy;
using Scripts.Game.Components.TurretSystem.Projectiles;
using UnityEngine;
using Zenject;

namespace Game.Pool
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private Transform _enemySpawnTransform;
        private ObjectPool<Bullet> _bulletPool;
        private ObjectPool<Stickman> _stickmanPool;
        private Vector3 stickmanTargetPoint;
        [Inject]
        private void OnInject()
        {
            _bulletPool = new ObjectPool<Bullet>(_bulletPrefab);
        }

        private void Start()
        {
            stickmanTargetPoint = Castle.Instance.CastleDoor;
        }
        
        
        
        public void SpawnBullet()
        {
            Bullet bullet = _bulletPool.Pull(Vector3.zero);
        }

        public void SpawnStickman()
        {
            Stickman stickman = _stickmanPool.Pull(_enemySpawnTransform.position, _enemySpawnTransform.rotation);
            stickman.Initialize(stickmanTargetPoint,100, 5);
        }
    }
}