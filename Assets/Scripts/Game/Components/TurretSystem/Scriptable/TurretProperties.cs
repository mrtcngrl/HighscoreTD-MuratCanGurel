using Scripts.Game.Components.TurretSystem.Turrets;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.Game.Components.TurretSystem.Scriptable
{
    [CreateAssetMenu(fileName = "New Turret", menuName = "Turret", order = 0)]
    public class TurretProperties : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private int _startPrice;
        [SerializeField] private GameObject _prefab;
        public float Damage;
        public float Range;
        public float Cooldown;
        public float CurrentPrice;
        public float StartPrice => _startPrice;
        public int ID => _id;
        public GameObject Prefab => _prefab;
    }
}