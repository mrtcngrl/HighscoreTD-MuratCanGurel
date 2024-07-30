using System.Collections.Generic;
using Scripts.Game.Components.TurretSystem.Projectiles;
using UnityEngine;
using Zenject;

namespace Game.Pool
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        private ObjectPool<Bullet> bulletPool;

        
        [Inject]
        private void OnInject()
        {
            bulletPool = new ObjectPool<Bullet>(_bulletPrefab);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                SpawnBullet();
        }
        
        
        public void SpawnBullet()
        {
            Bullet bullet = bulletPool.Pull(Vector3.zero);
        }
    }
}