using UnityEngine;

namespace Scripts.Game.Components.Enemy.Interface
{
    public interface IHittable
    {
        Transform Transform { get; }
        void OnHit(float damage);
    }
}