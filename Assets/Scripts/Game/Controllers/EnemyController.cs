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
        private float _interval = 3f;
        private float _modifiedInterval;
        private int _passedSeconds;
        private Spawner _spawner;

        private void Awake()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            GameConstants.OnFirstTurretPlaced += StartEnemyRush;
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

        private void Spawn()
        {
            _spawner.SpawnStickman();
            _passedSeconds += (int)_modifiedInterval;
            Debug.Log("Spawned " + _passedSeconds);
            if (_passedSeconds >= 60)
            {
                _enemySpawnCycle?.Dispose();
                Debug.LogError("Done !");
                return;
            }

            if (_passedSeconds >= 30 && _modifiedInterval >= 3f)
            {
                Debug.LogError("modified 2");
                _modifiedInterval = 2f;
                SetSpawnCycle(_modifiedInterval);
            }
            else if(_passedSeconds >= 45 && _modifiedInterval >= 2f)
            {
                Debug.LogError("modified 1");
                _modifiedInterval = 1f;
                SetSpawnCycle(_modifiedInterval);
            }
        }
    }
}