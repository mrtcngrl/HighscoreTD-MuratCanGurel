using UnityEngine;

namespace Scripts.Game.Components.Turret
{
    public abstract class TurretBase : MonoBehaviour
    {
        protected float Damage;
        protected float Range;
        protected bool IsActive;

        protected virtual void Initialize(float damage, float range)
        {
            Damage = damage;
            Range = range;
        }
        protected abstract void Activate();
        protected abstract void CheckArea();
        protected abstract void Fire();

       
        
        public virtual void OnPlace()
        {
            IsActive = true;
            Activate();
        }
        
        

    }
}