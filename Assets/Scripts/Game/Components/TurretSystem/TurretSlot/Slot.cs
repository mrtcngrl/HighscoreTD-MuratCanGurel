using Scripts.Game.Components.TurretSystem.Turrets;
using Scripts.Helpers;
using UnityEngine;

namespace Scripts.Game.Components.TurretSystem.TurretSlot
{
    public class Slot
    {
        private int _id;
        private bool _available;
        private Vector3 _position;
        private TurretBase _turret;
        public int ID => _id;
        public bool Available => _turret == null;
        public Vector3 Position => _position;
        public TurretBase Turret => _turret;
        public Slot(int id , bool available, Vector3 position, TurretBase turret = null)
        {
            _id = id;
            _available = available;
            _position = position;
            _turret = turret;
        }

        public void PlaceTurret(TurretBase turret)
        {
            _turret = turret;
            _turret.transform.position = Position;
            SlotController.Instance.SaveSlotData(this);
        }

        public void RemoveTurret()
        {
            _turret.gameObject.Destroy();
            _turret = null;
        }
    }
}