using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.EditorFramework
{

    /// <summary>
    /// 对组件的功能扩展
    /// </summary>
    public static class ComponentEx 
    {
        public static T GetOrAddComponent<T>(this Component self) where T : Component
        {
            return self.GetComponentOnly<T>() ?? self.gameObject.AddComponent<T>();
        }

        public static T GetComponentOnly<T>(this Component self) where T : Component
        {
            T result = self.GetComponent<T>();
            if (result == null) return default(T);
            return result;
        }
    }

}