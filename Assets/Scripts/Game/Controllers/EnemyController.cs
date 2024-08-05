using System;
using Game.Pool;
using Scripts.User;
using UniRx;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        private IDisposable _enemySpawnCycle;
        [SerializeField] private float _interval;
        [SerializeField] private float _minInterval;
        [SerializeField] private ParticleSystem _portalParticle;
        [SerializeField] private float _startHealth;
        private float _modifiedStartHealth;
        private float _modifiedInterval;
        private float _passedSeconds;
        private Spawner _spawner;
        private UserProgressData _userProgressData;
        private int _killedEnemyCount;

        public float StartHealth => _modifiedStartHealth;

        private void Subscribe()
        {
            GameConstants.OnFirstTurretPlaced += StartEnemyRush;
            GameConstants.OnEnemyDie += OnEnemyDie;
            GameConstants.OnSessionEnd += OnRetry;
            GameConstants.OnDataRecover += OnDataRecover;
        }
        
        [Inject]
        private void OnInject(Spawner spawner, UserProgressDataManager userProgressDataManager)
        {
            _spawner = spawner;
            Subscribe();
            _userProgressData = userProgressDataManager.Progress;
            _modifiedInterval = _interval;
            _userProgressData.SpawnInterval = _modifiedStartHealth = _startHealth;
        }

        private void OnRetry()
        {
            GameConstants.OnFirstTurretPlaced += StartEnemyRush;
            _modifiedInterval = _interval;
            _modifiedStartHealth = _startHealth;
            _enemySpawnCycle?.Dispose();
            _passedSeconds = 0;
            _killedEnemyCount = 0;
            _portalParticle.Stop();
        }

        private void StartEnemyRush()
        {
            GameConstants.OnFirstTurretPlaced -= StartEnemyRush;
            _portalParticle.Play();
            SetSpawnCycle(_modifiedInterval);
            _userProgressData.HasValue = true;
        }
        private void SetSpawnCycle(float delay)
        {
            //todo make async and wait for last cycle
            _enemySpawnCycle?.Dispose();
            _enemySpawnCycle = Observable.Timer(TimeSpan.FromSeconds(delay)).Repeat().Subscribe(_ => Spawn());
        }

        private void OnDestroy()
        {
            _enemySpawnCycle?.Dispose();
        }

        private void OnEnemyDie()
        {
            _modifiedStartHealth += 5;
            _userProgressData.EnemyDifficulty = (int)_modifiedStartHealth;
            if(_modifiedInterval <= _minInterval ) return;
            _killedEnemyCount++;
            if (_killedEnemyCount % 3 != 0) return;
            _modifiedInterval -= .25f;
            SetSpawnCycle(_modifiedInterval);

        }
        private void Spawn()
        {
            //todo change
            _spawner.SpawnStickman();
            _passedSeconds += _modifiedInterval;
            switch (_passedSeconds)
            {
                case >= 15 when _modifiedInterval >= 3f:
                    _modifiedInterval = 2.5f;
                    SetSpawnCycle(_modifiedInterval);
                    break;
                case >= 30 when _modifiedInterval >= 2.5f:
                    _modifiedInterval = 2f;
                    SetSpawnCycle(_modifiedInterval);
                    break;
                case >= 45 when _modifiedInterval >= 2f:
                    _modifiedInterval = 1.5f;
                    SetSpawnCycle(_modifiedInterval);
                    break;
                case >= 60 when _modifiedInterval > _minInterval:
                    _modifiedInterval = _minInterval;
                    break;
            }
            _userProgressData.SpawnInterval = _modifiedInterval;
        }

        private void OnDataRecover(UserProgressData userProgressData)
        {
            _modifiedInterval = _userProgressData.SpawnInterval = userProgressData.SpawnInterval;
            _modifiedStartHealth = _userProgressData.EnemyDifficulty = userProgressData.EnemyDifficulty;
            StartEnemyRush();
        }
    }
}