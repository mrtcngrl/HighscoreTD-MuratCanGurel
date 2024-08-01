using System;
using UnityEngine;

namespace Scripts.Game
{
    public class GameConstants
    {
        public static LayerMask Selectable;
        public static LayerMask Ground;
        public static LayerMask Enemy;
        public static float MaxDistanceToPlace = 2f;
        public static Action OnFirstTurretPlaced;
        public static Action OnEnemyDie;
        public static Action<int> CoinEarned;
        public static Action<int> CoinAmountChanged;
        public static Action<int> ScoreEarned;
        public static Action<int> ScoreChaged;
        public static Action OnSessionEnd;
        public static void Initialize()
        {
            Ground = 1 << LayerMask.NameToLayer("Ground");
            Selectable = 1 << LayerMask.NameToLayer("Selectable");
            Enemy = 1 << LayerMask.NameToLayer("Enemy");
        }
    }
}