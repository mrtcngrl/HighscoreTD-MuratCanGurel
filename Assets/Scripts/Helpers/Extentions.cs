using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Scripts.Helpers
{
    public static class Extensions
    {
        public static void Destroy(this Object @object)
        {
#if UNITY_EDITOR
            Object.DestroyImmediate(@object is Component component ? component.gameObject : @object);
#else
            Object.Destroy(@object is Component component ? component.gameObject : @object);
#endif
        }
        
        public static bool IsOverAnyUiElement(this Touch touch, int mask = -1) =>
            IsOverAnyUiElement(touch.position, mask);
        
        public static bool IsOverAnyUiElement(this Vector2 screenPosition, int mask = -1)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = screenPosition;
            List<RaycastResult> hitElements = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, hitElements);
            if (mask == -1) return hitElements.Any(x => x.module is GraphicRaycaster);

            foreach (var hit in hitElements)
            {
                if (!(hit.module is GraphicRaycaster)) continue;
                if ((1 << hit.gameObject.layer & mask) != 0) continue;
                return true;
            }

            return false;
        }

        public static bool IsOverAnyUiElement(this Vector2 screenPosition, string[] layersToIgnore) =>
            IsOverAnyUiElement(screenPosition, LayerMask.GetMask(layersToIgnore ?? Array.Empty<string>()));
        
        public static Vector3 CopyWithX(this Vector3 vector2, float x)
        {
            vector2.x = x;
            return vector2;
        }

        public static Vector3 CopyWithY(this Vector3 vector2, float y)
        {
            vector2.y = y;
            return vector2;
        }
    }
}