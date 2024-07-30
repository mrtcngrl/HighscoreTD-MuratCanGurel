using System;
using Scripts.Helpers;
using UnityEngine;

namespace Scripts.Game.Controllers
{
    public class SpawnController : MonoBehaviour
    {
        public static SpawnController Instance;
        private void Awake()
        {
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
        }
        
        public void SpawnTurretByID(int iD)
        {
            
        }
    }
}