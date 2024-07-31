using System;
using UniRx;
using UnityEngine;

namespace Scripts.Game.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        private IDisposable _enemySpawnCycle;
        private float _delay = 3f;
        private float _modifiedDelay;
        private int _passedSeconds;
        private void Awake()
        {
            GameConstants.OnFirstTurretPlaced += StartGame;
        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
                StartGame();
        }

        private void StartGame()
        {
            _modifiedDelay = _delay;
            _passedSeconds = 0;
            SetSpawnCycle(_delay);
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

        public void Spawn()
        {
            _passedSeconds += (int)_modifiedDelay;
            Debug.Log("Spawned " + _passedSeconds);
            if (_passedSeconds >= 60)
            {
                _enemySpawnCycle?.Dispose();
                Debug.LogError("Done !");
                return;
            }

            if (_passedSeconds >= 30 && _modifiedDelay >= 3f)
            {
                Debug.LogError("modified 2");
                _modifiedDelay = 2f;
                SetSpawnCycle(_modifiedDelay);
            }
            else if(_passedSeconds >= 45 && _modifiedDelay >= 2f)
            {
                Debug.LogError("modified 1");
                _modifiedDelay = 1f;
                SetSpawnCycle(_modifiedDelay);
            }
        }
    }
}