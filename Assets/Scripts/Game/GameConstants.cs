using UnityEngine;

namespace Scripts.Game
{
    public class GameConstants
    {
        public static LayerMask SelectableLayer;
        
        public static void Initialize()
        {
            SelectableLayer = 1 << LayerMask.NameToLayer("Selectable");
        }
    }
}