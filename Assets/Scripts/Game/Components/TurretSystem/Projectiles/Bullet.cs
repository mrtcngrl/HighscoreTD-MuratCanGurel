using System;
using Game.Pool;
using UnityEngine;

namespace Scripts.Game.Components.TurretSystem.Projectiles
{
    public class Bullet : MonoBehaviour, IPoolable<Bullet>
    {
        public void CacheAction(Action<Bullet> returnAction)
        {
            throw new NotImplementedException();
        }

        public void ReturnToPool()
        {
            throw new NotImplementedException();
        }
    }
}