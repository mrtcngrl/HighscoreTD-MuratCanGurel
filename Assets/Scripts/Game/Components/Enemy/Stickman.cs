using System;
using Game.Pool;
using UnityEngine;

namespace Scripts.Game.Components.Enemy
{
    public class Stickman : MonoBehaviour, IPoolable<Stickman>
    {
        private float _health;
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;
        public void Initialize(Vector3 targetPoint, float health, float damage)
        {
        }
        public void CacheAction(Action<Stickman> returnAction)
        {
            throw new NotImplementedException();
        }

        public void ReturnToPool()
        {
            throw new NotImplementedException();
        }
    }
}