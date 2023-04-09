using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoManager : Singleton<MonoManager>
{
    private MonoController controller;

    public MonoManager()
    {
        //��Ϊ����ģʽ���ú���ֻ��һ��
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
