
using System;
using System.Collections;
using Firebase.Database;
using Newtonsoft.Json;
using Scripts.Helpers;
using Scripts.User;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Controllers
{
    public class DatabaseController : MonoBehaviour
    {
        private string _userId;
        private DatabaseReference _dbReference;
        public static DatabaseController Instance;
        private UserProgressData _recoveredData;
        private UserProgressDataManager _userProgressDataManager;
        [Inject]
        private void OnInject(UserProgressDataManager userProgressDataManager)
        {
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
            _userProgressDataManager = userProgressDataManager;
        }
        private void Start()
        {
            _userId = SystemInfo.deviceUniqueIdentifier;
            _dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            StartCoroutine(TryLoadData());
        }
        
        private void SaveProgressData()
        {
            Debug.LogError("Try To save");
            string json = JsonUtility.ToJson(_userProgressDataManager.Progress);
            _dbReference.Child("users").Child(_userId).SetRawJsonValueAsync(json);
        }
        
        private void OnApplicationQuit()
        { 
            SaveProgressData();
        }

        private IEnumerator TryLoadData()
        {
            var userData = _dbReference.Child("users").Child(_userId).GetValueAsync();
            yield return new WaitUntil(predicate: () => userData.IsCompleted);
            if (userData != null)
            {
                DataSnapshot snapshot = userData.Result;
                string jsonData = snapshot.GetRawJsonValue();
                var progress = JsonUtility.FromJson<UserProgressData>(jsonData);
                if (progress.HasValue)
                {
                    _recoveredData = progress;
                    GameConstants.OnDataLoad?.Invoke();
                }
                    
            }
        }

        public void LoadRecoveredData()
        {
            GameConstants.OnDataRecover?.Invoke(_recoveredData);
            _userProgressDataManager.SetCoinAmount(_recoveredData.CoinAmount);
            _userProgressDataManager.SetScore(_recoveredData.Score);
        }  
        public void ClearData() => _recoveredData = null;
    }
}