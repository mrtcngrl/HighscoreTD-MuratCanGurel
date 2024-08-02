using System;
using UnityEngine;

namespace Scripts.Game
{
    public static class GameConstants
    {
        public const int StartCoinAmount = 500;
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
        public static Action<int> ScoreChaged;
        public static Action OnSessionEnd;
        public static Action OnRetry;
        public static Action OnBoosterUsed;
        public static Action OnBoosterEnd;
        public static void Initialize()
        {
            Ground = 1 << LayerMask.NameToLayer("Ground");
            Enemy = 1 << LayerMask.NameToLayer("Enemy");
        }
    }
}