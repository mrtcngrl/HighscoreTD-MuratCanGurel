using UnityEngine;

namespace Scripts.Game.Components.TurretSystem.Turrets
{
    public class Turret: TurretBase
    {
        protected override void Activate()
        {
            //todo do some magic
        }

        protected override void CheckArea()
        {
            throw new System.NotImplementedException();
        }

        protected override void Fire()
        {
            throw new System.NotImplementedException();
        }

        public void ButtonClicked()
        {
            Debug.LogError("ButtonClicked");
        }
    }
}