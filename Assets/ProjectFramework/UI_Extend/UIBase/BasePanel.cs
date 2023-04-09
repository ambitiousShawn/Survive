using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shawn.ProjectFramework
{

    /// <summary>
    /// ���Panel�Ļ��ࣺ
    ///     1.�洢���������������Ϣ(�ֵ�)
    ///     2.�����ṩ��ʼ������ʾ / ���ظ�����API�ӿ�
    ///     3.�����ṩ��չ ������(��ť / ������)�ļ���ί�лص�
    ///     4.�����ṩѰ�����������Ľӿ�
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