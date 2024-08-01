using System;
using Game.Pool;
using UniRx;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        private IDisposable _enemySpawnCycle;
        [SerializeField] private float _interval = 3f;
        [SerializeField] private ParticleSystem _portalParticle;
        [SerializeField] private float _health;
        private float _modifiedInterval;
        private float _passedSeconds;
        private Spawner _spawner;

        public float Health => _health;
        private void Awake()
        {
            Subscribe();
            _modifiedInterval = _interval;
        }

        private void Subscribe()
        {
            GameConstants.OnFirstTurretPlaced += StartEnemyRush;
            GameConstants.OnEnemyDie += OnEnemyDie;
        }
        
        private void Unsubscribe()
        {
            GameConstants.OnFirstTurretPlaced -= StartEnemyRush;
        }

        [Inject]
        private void OnInject(Spawner spawner)
        {
            _spawner = spawner;
        }

        public void Reset()
        {
            Subscribe();
            _modifiedInterval = _interval;
            _passedSeconds = 0;
        }

        private void StartEnemyRush()
        {
            Unsubscribe();
            _portalParticle.Play();
            SetSpawnCycle(_interval);
        }
        private void SetSpawnCycle(float delay)
        {
            _enemySpawnCycle?.Dispose();
            _enemySpawnCycle = Observable.Timer(TimeSpan.FromSeconds(delay)).Repeat().Subscribe(_ => Spawn());
        }

        private void OnDestroy()
        {
            _enemySpawnCycle?.Dispose();
        }

        private void OnEnemyDie()
        {
            _health += 10;
            if(_modifiedInterval <= 1 || _passedSeconds >= 60) return;
            _modifiedInterval -= .25f;
            SetSpawnCycle(_modifiedInterval);
        }
        private void Spawn()
        {
            _spawner.SpawnStickman();
            _passedSeconds += _modifiedInterval;
            Debug.Log("Spawned " + _passedSeconds);
            if (_passedSeconds >= 60)
            {
                _enemySpawnCycle?.Dispose();
                return;
            }

            if (_passedSeconds >= 30 && _modifiedInterval >= 5f)
            {
                _modifiedInterval = 4f;
                SetSpawnCycle(_modifiedInterval);
            }
            else if(_passedSeconds >= 45 && _modifiedInterval >= 4f)
            {
                _modifiedInterval = 3f;
                SetSpawnCycle(_modifiedInterval);
            }
        }
    }
}