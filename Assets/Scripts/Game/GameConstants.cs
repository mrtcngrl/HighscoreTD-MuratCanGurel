using System;
using UnityEngine;

namespace Scripts.Game
{
    public class GameConstants
    {
        public static LayerMask Selectable;
        public static LayerMask Ground;
        public static float MaxDistanceToPlace = 2f;
        public static Action OnFirstTurretPlaced;
        public static void Initialize()
        {
            Ground = 1 << LayerMask.NameToLayer("Ground");
            Selectable = 1 << LayerMask.NameToLayer("Selectable");
        }
    }
}