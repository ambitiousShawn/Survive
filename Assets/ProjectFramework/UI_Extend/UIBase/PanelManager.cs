using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shawn.ProjectFramework
{

    /// <summary>
    /// 面板UI管理器，提供以下功能接口：
    ///     1.字典存储当前显示的所有面板Panel。
    ///     2.对外提供 初始化 / 显示 / 隐藏 面板的API接口。
    ///     3.对外提供根据名称查找面板的接口。
    /// </summary>
    public class PanelManager : IBaseManager
    {
        public const string UI_Panel_Path = "UI/Panel/";
        public const string UI_Base_Path = "UI/";
        public static PanelManager Instance;
        private bool m_IsInit;
        private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
        private Transform canvas, eventSystem;

        public void Init()
        {
            if (m_IsInit) return;

            m_IsInit = true;
            Instance = this;

            //Canvas 和 EventSystem的初始化
            GameObject canvasObj = ResourcesManager.Instance.Load<GameObject>(UI_Base_Path + "Canvas");
            GameObject eventSystemObj = ResourcesManager.Instance.Load<GameObject>(UI_Base_Path + "EventSystem");
            canvasObj.name = "Canvas";
            eventSystemObj.name = "EventSystem";
            canvas = canvasObj.transform;
            eventSystem = canvasObj.transform;
            GameObject.DontDestroyOnLoad(canvasObj);
            GameObject.DontDestroyOnLoad(eventSystemObj);
        }

    /*测试程序*/
        public void Tick()
        {
            /*# region 测试
            if (Input.GetKeyDown(KeyCode.M))
            {
                PanelManager.Instance.ShowPanel<UGUI_BeginPanel>("UGUI_BeginPanel");
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                PanelManager.Instance.ShowPanel<UGUI_MainUIPanel>("UGUI_MainUIPanel");
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                DialogueManager.Instance.ShowDialogue(0);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                DialogueManager.Instance.ShowDialogue(5);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                BuffManager.Instance.AddBuff(0, () =>
                {
                    Debug.Log("和风守护buff已触发");
                });
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                BuffManager.Instance.AddBuff(1, () =>
                {
                    Debug.LogError("烧伤Debuff已触发");
                });
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                BuffManager.Instance.AddBuff(2, () =>
                {
                    Debug.LogWarning("冻伤Debuff已触发");
                });
            }
            # endregion 测试*/
        }

        public BasePanel GetPanelByName (string panelName)
        {
            if (panelDic.TryGetValue(panelName, out tempPanel))
            {
                return tempPanel;
            }
            return null;
        }

        /// <summary>
        /// 显示面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panelName">要显示的面板名称</param>
        /// <param name="constant">显示面板是否长期留存(默认长期留存)</param>
        /// <param name="delta">如果不长期留存，留存的时间(默认1s留存时长)</param>
        public void ShowPanel<T> (string panelName, UnityAction callback = null, bool constant = true, float delta = 1f) where T : BasePanel
        {
            if (panelDic.ContainsKey(panelName)) return;

            ResourcesManager.Instance.LoadAsync<GameObject>(UI_Panel_Path + panelName, (panel) =>
            {
                panel.transform.SetParent(canvas);
                panel.transform.localPosition = Vector3.zero;
                panel.transform.localScale = Vector3.one;
                panel.name = panelName;

                T script = panel.GetComponent<T>();
                script.Show();

                panelDic.Add(panelName, script); 
            });

            callback?.Invoke();

            if (!constant)
            {
                HidePanel(panelName, delta);
            }
        }

        private BasePanel tempPanel = null;

        /// <summary>
        /// 隐藏面板
        /// </summary>
        /// <param name="panelName">面板名称</param>
        /// <param name="timer">倒计时</param>
        public void HidePanel(string panelName, float timer = 0)
        {
            MonoManager.Instance.StartCoroutine(DelayHide(panelName, timer));
        }

        IEnumerator DelayHide(string panelName, float timer)
        {
            yield return new WaitForSeconds(timer);
            if (panelDic.TryGetValue(panelName, out tempPanel))
            {
                GameObject.Destroy(tempPanel.gameObject);
                panelDic.Remove(panelName);
            }
        }
    }
}