using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.ProjectFramework
{

    public class CardProcessManager : IBaseManager
    {
        public static CardProcessManager Instance;
        private const string Plot_Line_Path = "SO/Card_SO";

        public List<CardNode> nodes;
        public int pos;

        private CardNode newCard;
        

        public void Init()
        {
            Instance = this;
            nodes = Resources.Load<CardData_SO>(Plot_Line_Path).Nodes;
            pos = 0;
        }

        public void Tick()
        {
            /*if (Input.GetKeyDown(KeyCode.H))
            {
                JumpToPosition(0);
                Debug.Log("当前位置名字为：" + nodes[pos].Name);
            }*/
        }

        /// <summary>
        /// 从外部调用，传入的target是基于自身回复内容的下标。
        /// </summary>
        /// <param name="targetPos">跳转到第一条回复位置即填入0，以此类推累加即可。</param>
        public void JumpToPosition(int targetNum)
        {
            NextInfo nextInfo = nodes[pos].NextList[targetNum];
            JumpToAllPosition(nextInfo.TargetID);
        }

        /// <summary>
        /// 跳转到剧情线的某处targetPos
        /// </summary>
        /// <param name="targetPos">任意跳转位置</param>
        public void JumpToAllPosition(int targetAllPos)
        {
            newCard = null;
            //TODO:销毁之前的卡牌            

            pos = targetAllPos; //修改位置指针

            //初始化新的卡牌
            newCard = InitCardNode(pos);
            //TODO:调用表现层更新逻辑即可
        }

        /// <summary>
        /// 初始化当前位置的卡牌信息(仅限数据层)
        /// </summary>
        /// <param name="currPos"></param>
        /// <returns>返回初始化后的卡牌信息</returns>
        private CardNode InitCardNode(int currPos)
        {
            return nodes[currPos];
        }
    }

}