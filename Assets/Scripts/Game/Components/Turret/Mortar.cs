using UnityEngine;

namespace Scripts.Game.Components.Turret
{
    public class Mortar : TurretBase
    {
        [SerializeField, Range(0,1)] private float _deadRangePercentage;
        private float _deadRange;

        protected override void Initialize(float damage, float range)
        {
            base.Initialize(damage, range);
            _deadRange = range * _deadRangePercentage;
        }

        protected override void CheckArea()
        {
            throw new System.NotImplementedException();
        }

        protected override void Fire()
        {
            throw new System.NotImplementedException();
        }

        protected override void Activate()
        {
            throw new System.NotImplementedException();
        }
    }
}