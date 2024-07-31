using Scripts.Helpers;
using UnityEngine;
using Zenject;

namespace Scripts.Game.Components
{
    public class Castle : MonoBehaviour
    {
        public static Castle Instance;
        [HideInInspector] public Vector3 CastleDoor;
        [Inject]
        private void OnInject()
        {
            CastleDoor = (transform.position + Vector3.forward * .5f).CopyWithY(0);
            if (!object.ReferenceEquals(Instance, null) && !object.ReferenceEquals(Instance, this)) this.Destroy();
            else
            {
                Instance = this;
            }
        }
    }
}