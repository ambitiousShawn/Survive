using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shawn.ProjectFramework
{

    /// <summary>
    /// ���UI���������ṩ���¹��ܽӿڣ�
    ///     1.�ֵ�洢��ǰ��ʾ���������Panel��
    ///     2.�����ṩ ��ʼ�� / ��ʾ / ���� ����API�ӿڡ�
    ///     3.�����ṩ�������Ʋ������Ľӿڡ�
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

            //Canvas �� EventSystem�ĳ�ʼ��
            GameObject canvasObj = ResourcesManager.Instance.Load<GameObject>(UI_Base_Path + "Canvas");
            GameObject eventSystemObj = ResourcesManager.Instance.Load<GameObject>(UI_Base_Path + "EventSystem");
            canvasObj.name = "Canvas";
            eventSystemObj.name = "EventSystem";
            canvas = canvasObj.transform;
            eventSystem = canvasObj.transform;
            GameObject.DontDestroyOnLoad(canvasObj);
            GameObject.DontDestroyOnLoad(eventSystemObj);
        }

    /*���Գ���*/
        public void Tick()
        {
            
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
        /// ��ʾ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panelName">Ҫ��ʾ���������</param>
        /// <param name="constant">��ʾ����Ƿ�������(Ĭ�ϳ�������)</param>
        /// <param name="delta">������������棬�����ʱ��(Ĭ��1s����ʱ��)</param>
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
        /// �������
        /// </summary>
        /// <param name="panelName">�������</param>
        /// <param name="timer">����ʱ</param>
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