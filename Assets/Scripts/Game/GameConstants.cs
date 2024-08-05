using System;
using Scripts.User;
using UnityEngine;

namespace Scripts.Game
{
    public static class GameConstants
    {
        public const int StartCoinAmount = 50;
        public const float BoosterIncreasePercent = .1f;
        public const float BoosterDuration = 5f;
        public static LayerMask Ground;
        public static LayerMask Enemy;
        public const float MaxDistanceToPlace = 2f;
        public static Action OnFirstTurretPlaced;
        public static Action OnEnemyDie;
        public static Action<int> CoinEarned;
        public static Action<int> CoinAmountChanged;
        public static Action<int> ScoreEarned;
        public static Action<int> ScoreChanged;
        public static Action<UserProgressData> OnDataRecover;
        public static Action OnDataLoad;
        public static Action OnSessionEnd;
        public static Action OnRetry;
        public static void Initialize()
        {
            Ground = 1 << LayerMask.NameToLayer("Ground");
            Enemy = 1 << LayerMask.NameToLayer("Enemy");
        }
    }
}