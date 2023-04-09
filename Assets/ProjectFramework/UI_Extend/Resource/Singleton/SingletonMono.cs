using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject manager = new GameObject();
                manager.name = typeof(T).ToString();
                instance = manager.AddComponent<T>();

                DontDestroyOnLoad(manager);
            }
            return instance;
        }
    }
}
