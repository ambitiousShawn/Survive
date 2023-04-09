using Shawn.EditorFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shawn.ProjectFramework
{
    /// <summary>
    /// Buff管理器：
    ///     1.对外提供添加Buff的API接口，带持续时长，效果实现，更新UI，触发间隔。
    ///     2.对外提供删除Buff的API接口。
    ///     3.对外提供查询Buff的API接口。
    /// </summary>
    public class BuffManager : IBaseManager
    {
        class TimerInfo
        {
            public float mainTimer;
            public float stageTimer;

            public void ReturnZero()
            {
                mainTimer = 0;
                stageTimer = 0;
            }
        }

        public const string Buff_Path = "SO/Buff_SO";
        public static BuffManager Instance;

        public Dictionary<string, BuffNode> buffDic = new Dictionary<string, BuffNode>();
        private TimerInfo[] timerInfo = new TimerInfo[10];
        private List<string> deleteBuff = new List<string>();
        private List<BuffNode> buffList;

        private UGUI_MainUIPanel panel;

        public void Init()
        {
            Instance = this;
            buffList = Resources.Load<BuffData_SO>(Buff_Path).Nodes;

            //初始化计时器组
            for (int i = 0;i < timerInfo.Length; i++)
            {
                timerInfo[i] = new TimerInfo();
                timerInfo[i].ReturnZero();
            }
        }

        /// <summary>
        /// 帧更新
        /// </summary>
        public void Tick()
        {
            if (buffDic.Count <= 0) return;

            TimerInfo info;
            BuffNode buff;
            foreach (string name in buffDic.Keys)
            {
                buff = buffDic[name];
                int id = buff.ID;
                info = timerInfo[id];
                info.mainTimer += Time.deltaTime;
                info.stageTimer += Time.deltaTime;
                if (info.mainTimer > buff.Time)
                {
                    deleteBuff.Add(name);
                    timerInfo[id].ReturnZero();
                }
                else
                {
                    if (info.stageTimer > buff.Interval)
                    {
                        buff.Callback();
                        timerInfo[id].stageTimer = 0;
                    }
                }
            }

            for (int i = 0;i < deleteBuff.Count; i++)
            {
                string deleteName = deleteBuff[i];
                buffDic.Remove(deleteName);
                deleteBuff.Remove(deleteName);
                if (panel != null) panel.UpdateBuffFrame();
            }
        }

        /// <summary>
        /// 将对应id的BUFF加到玩家状态栏中，并传入其对应的效果
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callback"></param>
        public void AddBuff(int id, UnityAction callback)
        {
            
            BuffNode node = buffList[id];
            node.Callback = callback; //注入Buff单次触发事件
            //存入字典后并更新UI
            if (!buffDic.ContainsKey(node.Name))
            {
                buffDic.Add(node.Name, node);
                panel = PanelManager.Instance.GetPanelByName("UGUI_MainUIPanel") as UGUI_MainUIPanel;
                if (panel != null)
                    panel.UpdateBuffFrame();
                callback?.Invoke();
            }
            else
            {
                timerInfo[id].mainTimer = 0;
            }
        }

        /// <summary>
        /// 根据名字查询当前玩家拥有的Buff
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BuffNode FindBuffByName(string name)
        {
            if (buffDic.ContainsKey(name))
            {
                return buffDic[name];
            }
            return null;
        }
    }

}