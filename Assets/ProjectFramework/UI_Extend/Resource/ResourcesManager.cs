using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesManager : Singleton<ResourcesManager>
{
    //ͬ��������Դ
    /*
        Param1:�ļ���ResourcesĿ¼�µ�·��
    */
    public T Load<T>(string path) where T : Object
    {
        T res = Resources.Load<T>(path);
        //���˵T������GameObject�����ڳ������ʼ��һ��
        if (res is GameObject)  return GameObject.Instantiate(res);
        return res;
    }

    //�첽������Դ
    /*
        Param1:�ļ���ResourcesĿ¼�µ�·��
    */
    public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        MonoManager.Instance.StartCoroutine(IE_LoadAsync(path, callback));
    }

    //�첽����Э��
    private IEnumerator IE_LoadAsync<T>(string path,UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(path);
        yield return r;
        if (r.asset is GameObject)  callback(GameObject.Instantiate<T>(r.asset as T));
        else  callback(r.asset as T);
    }
}
