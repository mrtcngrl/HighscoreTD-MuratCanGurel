using System;
using Game.Pool;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Scripts.Game.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        private IDisposable _enemySpawnCycle;
        [SerializeField] private float _interval = 3f;
        [SerializeField] private ParticleSystem _portalParticle;
        [SerializeField] private float _startHealth;
        private float _modifiedStartHealth;
        private float _modifiedInterval;
        private float _passedSeconds;
        private Spawner _spawner;

        public float StartHealth => _modifiedStartHealth;

        private void Subscribe()
        {
            GameConstants.OnFirstTurretPlaced += StartEnemyRush;
            GameConstants.OnEnemyDie += OnEnemyDie;
            GameConstants.OnSessionEnd += Reset;
        }
        
        [Inject]
        private void OnInject(Spawner spawner)
        {
            _spawner = spawner;
            Subscribe();
            _modifiedInterval = _interval;
            _modifiedStartHealth = _startHealth;
        }

        public void Reset()
        {
            GameConstants.OnFirstTurretPlaced += StartEnemyRush;
            _modifiedInterval = _interval;
            _enemySpawnCycle?.Dispose();
            _passedSeconds = 0;
        }

        private void StartEnemyRush()
        {
            GameConstants.OnFirstTurretPlaced -= StartEnemyRush;
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
            _modifiedStartHealth += 20;
            if(_modifiedInterval <= .75f || _passedSeconds >= 60) return;
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