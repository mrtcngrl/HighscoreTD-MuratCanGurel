using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Pool;
using Scripts.Game.Components.Enemy.Interface;
using Scripts.Helpers;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Scripts.Game.Components.TurretSystem.Projectiles
{
    public class Projectile : MonoBehaviour, IPoolable<Projectile>
    {
        [Serializable]
        public class ProjectileInfo
        {
            public ProjectileType ProjectileType;
            public GameObject ProjectileModel;
            public ParticleSystem ImpactParticle;
            public float Speed;
            public float LifeTime;
        }

        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _lifeTime;
        [SerializeField] private List<ProjectileInfo> _projectilesInfo = new();
        private Transform _transform;
        private ProjectileInfo _currentProjectile;
        private Action<Projectile> _returnAction;
        private float _damage;
        private IDisposable _lifeTimer;

        private void Awake()
        {
            _transform = transform;
        }

        public void CacheAction(Action<Projectile> returnAction)
        {
            _returnAction = returnAction;
        }

        public void ReturnToPool()
        {
            _lifeTimer?.Dispose();
            _returnAction?.Invoke(this);
            _rb.isKinematic = false;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        public void Launch(ProjectileType projectileType, Vector3 targetPosition, float damage)
        {
            _damage = damage;
            Prepare(projectileType);
            switch (projectileType)
            {
                case ProjectileType.Bullet:
                    _rb.isKinematic = false;
                    Vector3 dir = (targetPosition.CopyWithY(.5f) - _transform.position).normalized;
                    _rb.velocity = dir * _currentProjectile.Speed;
                    DisposeAndSetLifeTimer(_lifeTime);
                    break;
                case ProjectileType.Mine:
                    _rb.isKinematic = true;
                    _transform.DOJump(targetPosition.CopyWithY(.1f), 2f, 1, 1f);
                    break;
                case ProjectileType.Mortar:
                    float diff = (_transform.position - targetPosition).sqrMagnitude;
                    float duration = diff / _currentProjectile.Speed;
                    _rb.isKinematic = true;
                    _transform.DOJump(targetPosition.CopyWithY(.2f), 2f, 1, duration).OnComplete(()=>
                    {
                        ProvideAreaDamage(1);
                    });
                    break;
            }
            
        }
        
        private void Prepare(ProjectileType projectileType)
        {
            foreach (var projectileInfo in _projectilesInfo)
            { 
                bool toggle = projectileInfo.ProjectileType == projectileType;
                if (toggle)
                    _currentProjectile = projectileInfo;
                projectileInfo.ProjectileModel.SetActive(toggle);
            }
        }

      
        private void OnTriggerEnter(Collider other)
        {
            IHittable hitObject = other.attachedRigidbody?.GetComponent<IHittable>();
            if(hitObject == null) return;
            switch (_currentProjectile.ProjectileType)
            {
                case ProjectileType.Bullet:
                   hitObject.OnHit(_damage);
                   _rb.velocity = Vector3.zero;
                   _currentProjectile.ProjectileModel.SetActive(false);
                   _currentProjectile.ImpactParticle.Play();
                    break;
                case ProjectileType.Mine:
                    ProvideAreaDamage(1);
                    break;
            }
            
        }
        
        private void ProvideAreaDamage(float radius)
        {
            var Colliders = Physics.OverlapSphere(transform.position, 1, GameConstants.Enemy).ToList();
            foreach (var collider in Colliders)
            {
                collider.attachedRigidbody?.GetComponent<IHittable>().OnHit(_damage);
            }
            _currentProjectile.ProjectileModel.SetActive(false);
            _currentProjectile.ImpactParticle.Play();
            DisposeAndSetLifeTimer(1f);
        }
        
        private void DisposeAndSetLifeTimer(float delay)
        {
            _lifeTimer?.Dispose();
            _lifeTimer = Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ => ReturnToPool());
        }
    }
}