using UnityEngine;

namespace Scripts.Game.Components.TurretSystem.Turrets
{
    public class Mortar : TurretBase
    {
        [SerializeField, Range(0,1)] private float _deadRangePercentage;
        private float _deadRange;

        protected override void Initialize()
        {
            base.Initialize();
            _deadRange = Range * _deadRangePercentage;
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