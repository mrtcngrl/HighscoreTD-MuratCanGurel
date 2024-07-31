using System;
using DG.Tweening;
using Game.Pool;
using Scripts.Game.Components.Enemy.Interface;
using UnityEngine;

namespace Scripts.Game.Components.Enemy
{
    public class Stickman : MonoBehaviour, IPoolable<Stickman>, IHittable
    {
        private float _health;
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;
        [SerializeField] private Transform _transform;
        private Tween _walkTween;
        private Action<Stickman> _returnAction;
        private void Awake()
        {
            _transform = transform;
        }

        public void Initialize(Vector3 targetPoint, float health, float damage)
        {
            _health = health;
            _damage = damage;
            float duration = Vector3.Distance(_transform.position, targetPoint) / _speed;
            _walkTween = _transform.DOMove(targetPoint, duration).SetEase(Ease.Linear).OnComplete(Explode);
        }

        public Transform Transform => _transform;

        public void OnHit(float damage)
        {
            _health -= damage;
            Debug.LogError("Hit " + damage);
            
        }
        private void Die()
        {
            _walkTween?.Kill();
        }
        private void Explode()
        {
            _returnAction?.Invoke(this);
        }
        public void CacheAction(Action<Stickman> returnAction)
        {
            _returnAction = returnAction;
        }

        public void ReturnToPool()
        {
            throw new NotImplementedException();
        }
    }
}