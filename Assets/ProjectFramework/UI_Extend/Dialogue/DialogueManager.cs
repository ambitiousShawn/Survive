using Shawn.EditorFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.ProjectFramework
{
    /// <summary>
    /// 利用ScriptableObject实现对话存储的 对话系统管理器：
    /// </summary>
    public class DialogueManager : IBaseManager
    {
        public const string Dialogue_Path = "SO/Dialogue_SO";
        public static DialogueManager Instance;

        private bool m_IsInit;
        private bool m_CanStep;
        private bool m_Roll;
        private bool m_isRolling;
        public int pos;
        private DialogueData_SO dialogue_SO;
        public List<DialogueNode> tempList ;

        public BasePanel temp = null;

        public void Init()
        {
            if (m_IsInit) return;

            m_IsInit = true;
            m_CanStep = false;
            m_Roll = true;
            m_isRolling = false;
            Instance = this;
            pos = 0;
            dialogue_SO = Resources.Load<DialogueData_SO>(Dialogue_Path);
            tempList = dialogue_SO.Nodes;
        }

        public void Tick()
        {
            if (m_CanStep && Input.GetKeyDown(KeyCode.Space))
            {
                Step();
            }
        }

        public void ShowDialogue(int pos)
        {
            m_CanStep = true;
            this.pos = pos;

            if (temp == null)
            {
                PanelManager.Instance.ShowPanel<UGUI_DialoguePanel>("UGUI_DialoguePanel");
            }
        }

        

        private void Step()
        {
            //由于异步加载，变量赋值需放在显示面板后
            if (temp == null)
                temp = PanelManager.Instance.GetPanelByName("UGUI_DialoguePanel");

            //如果文字正在滚动，将文字补全
            if (m_isRolling)
            {
                (temp as UGUI_DialoguePanel).UpdateInfo(tempList[pos].Info);
                m_isRolling = false;
                MonoManager.Instance.StopAllCoroutines();
                return;
            }

            pos++; //TODO:后续会改动跳转位置(对于多选项多结局对话)

            //如果对话越界或为空，直接隐藏对话框
            if (pos >= tempList.Count || string.IsNullOrEmpty(tempList[pos].Info))
            {
                PanelManager.Instance.HidePanel("UGUI_DialoguePanel");
                m_CanStep = false;
                return ;
            }


            if (!m_Roll)
            {
                (temp as UGUI_DialoguePanel).UpdateInfo(tempList[pos].Info);
            }
            else
            {
                MonoManager.Instance.StartCoroutine(RollText(temp));
            }
        }

        public IEnumerator RollText (BasePanel temp)
        {
            m_isRolling = true;
            (temp as UGUI_DialoguePanel).UpdateInfo("");
            foreach (char c in dialogue_SO.Nodes[pos].Info)
            {
                (temp as UGUI_DialoguePanel).AppendInfo(c);
                yield return new WaitForSeconds(0.05f);
            }
            m_isRolling = false;
        }
    }
}