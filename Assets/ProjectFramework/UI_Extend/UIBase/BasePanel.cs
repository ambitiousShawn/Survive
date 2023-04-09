using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shawn.ProjectFramework
{

    /// <summary>
    /// 面板Panel的基类：
    ///     1.存储该面板的所有组件信息(字典)
    ///     2.对外提供初始化，显示 / 隐藏该面板的API接口
    ///     3.对外提供扩展 相关组件(按钮 / 滑动条)的监听委托回调
    ///     4.对外提供寻找面板内组件的接口
    /// </summary>
    public abstract class BasePanel : MonoBehaviour
    {
        protected Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

        public virtual void Init()
        {
            FindChildControl<Text>();
            FindChildControl<Button>();
            FindChildControl<Image>();
            FindChildControl<Toggle>();
            FindChildControl<Slider>();
            FindChildControl<Scrollbar>();
        }

        public virtual void Show()
        {
            Init();
        }

        public virtual void Hide()
        {

        }

        private void FindChildControl<T>() where T : UIBehaviour
        {
            T[] controls = GetComponentsInChildren<T>();

            foreach (T control in controls)
            {
                string objName = control.gameObject.name;
                if (controlDic.ContainsKey(control.gameObject.name))
                    controlDic[objName].Add(control);
                else
                    controlDic.Add(objName, new List<UIBehaviour>() { control });

                if (control is Button)
                {
                    (control as Button).onClick.AddListener(() =>
                    {
                        OnClick(objName);
                    });
                }else if (control is Toggle)
                {
                    (control as Toggle).onValueChanged.AddListener((value) =>
                    {
                        OnValueChanged(objName, value);
                    });
                }else if (control is Slider)
                {
                    (control as Slider).onValueChanged.AddListener((value) =>
                    {
                        OnValueChanged(objName, value);
                    });
                }
            }
        }

        protected virtual void OnClick(string objName) { }

        protected virtual void OnValueChanged(string objName, bool value) { }

        protected virtual void OnValueChanged(string objName, float value) { }

        protected T GetControl<T>(string controlName) where T : UIBehaviour
        {
            if (controlDic.ContainsKey(controlName))
            {
                foreach (UIBehaviour control in controlDic[controlName])
                {
                    if (control is T) return control as T;
                }
            }
            return null;
        }
    }

    

}