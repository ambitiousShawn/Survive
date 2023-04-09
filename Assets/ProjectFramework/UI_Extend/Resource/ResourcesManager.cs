using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesManager : Singleton<ResourcesManager>
{
    //同步加载资源
    /*
        Param1:文件在Resources目录下的路径
    */
    public T Load<T>(string path) where T : Object
    {
        T res = Resources.Load<T>(path);
        //如果说T类型是GameObject，其在场景会初始化一份
        if (res is GameObject)  return GameObject.Instantiate(res);
        return res;
    }

    //异步加载资源
    /*
        Param1:文件在Resources目录下的路径
    */
    public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        MonoManager.Instance.StartCoroutine(IE_LoadAsync(path, callback));
    }

    //异步加载协程
    private IEnumerator IE_LoadAsync<T>(string path,UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(path);
        yield return r;
        if (r.asset is GameObject)  callback(GameObject.Instantiate<T>(r.asset as T));
        else  callback(r.asset as T);
    }
}
