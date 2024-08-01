using System;
using DG.Tweening;
using Game.Pool;
using Scripts.Game.Components.Enemy.Interface;
using UniRx;
using UnityEngine;

namespace Scripts.Game.Components.Enemy
{
    public class Stickman : MonoBehaviour, IPoolable<Stickman>, IHittable
    {
        private float _health;
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;
        [SerializeField] private int _price;
        [SerializeField] private Transform _transform;
        [SerializeField] private GameObject _model;
        [SerializeField] private ParticleSystem deathParticle;
        private Tween _walkTween;
        private Action<Stickman> _returnAction;
        private IDisposable returnTimer;
        private void Awake()
        {
            _transform = transform;
        }

        public void Initialize(Vector3 targetPoint, float health, float damage, int price)
        {
            _price = price;
            _health = health;
            _damage = damage;
            float duration = Vector3.Distance(_transform.position, targetPoint) / _speed;
            _walkTween = _transform.DOMove(targetPoint, duration).SetEase(Ease.Linear).OnComplete(Explode);
        }

        public Transform Transform => _transform;

        public void OnHit(float damage)
        {
            _health -= damage;
            if(_health <= 0)
                Die();
        }
        private void Die()
        {
            _walkTween?.Kill();
            _model.SetActive(false);
            deathParticle.Play();
            returnTimer?.Dispose();
            returnTimer = Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_=>ReturnToPool());
            GameConstants.CoinEarned?.Invoke(_price);
            GameConstants.ScoreEarned?.Invoke(100);
        }
        private void Explode()
        {
            ReturnToPool();
        }
        public void CacheAction(Action<Stickman> returnAction)
        {
            _returnAction = returnAction;
            _model.SetActive(true);
            returnTimer?.Dispose();
        }

        public void ReturnToPool()
        {
            _returnAction?.Invoke(this);
        }
    }
}