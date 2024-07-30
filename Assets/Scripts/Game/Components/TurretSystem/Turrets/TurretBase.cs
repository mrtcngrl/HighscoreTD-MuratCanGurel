using Scripts.Game.Components.TurretSystem.Scriptable;
using Scripts.Game.Controllers;
using Scripts.Helpers;
using UnityEngine;

namespace Scripts.Game.Components.TurretSystem.Turrets
{
    public abstract class TurretBase : MonoBehaviour, ISelectable
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


        public Vector3 Position => transform.position;
        public bool Available => !IsActive;
        
        public void OnHold(Vector3 mouseWorldPos)
        {
            var candidatePosition = mouseWorldPos + Vector3.forward;
            candidatePosition = candidatePosition.CopyWithY(1.5f);
            transform.position = candidatePosition;
        }

        public void OnRelease(out bool placed)
        {
            placed = false;
        }
    }
}