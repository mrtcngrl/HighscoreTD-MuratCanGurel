using Scripts.Game.Components.TurretSystem.Scriptable;
using UnityEngine;

namespace Scripts.Game.Components.TurretSystem.Turrets
{
    public abstract class TurretBase : MonoBehaviour
    {
        [SerializeField] protected TurretProperties _properties;
        protected float Damage;
        protected float Range;
        protected float Cooldown;
        protected bool IsActive;

        protected virtual void Initialize()
        {
            Damage = _properties.Damage;
            Range = _properties.Range;
            Cooldown = _properties.Cooldown;
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