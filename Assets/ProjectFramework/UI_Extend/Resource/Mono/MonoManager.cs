using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoManager : Singleton<MonoManager>
{
    private MonoController controller;

    public MonoManager()
    {
        //因为单例模式，该函数只进一次
        if (controller == null) controller = new GameObject("MonoController").AddComponent<MonoController>();
    }

    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }

    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }

    public void StopAllCoroutines()
    {
        controller.StopAllCoroutines();
    }

    public void Invoke(string methodName, float time)
    {
        controller.Invoke(methodName, time);
    }
}
