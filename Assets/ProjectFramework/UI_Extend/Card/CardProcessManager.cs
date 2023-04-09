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
                Debug.Log("��ǰλ������Ϊ��" + nodes[pos].Name);
            }*/
        }

        /// <summary>
        /// ���ⲿ���ã������target�ǻ�������ظ����ݵ��±ꡣ
        /// </summary>
        /// <param name="targetPos">��ת����һ���ظ�λ�ü�����0���Դ������ۼӼ��ɡ�</param>
        public void JumpToPosition(int targetNum)
        {
            NextInfo nextInfo = nodes[pos].NextList[targetNum];
            JumpToAllPosition(nextInfo.TargetID);
        }

        /// <summary>
        /// ��ת�������ߵ�ĳ��targetPos
        /// </summary>
        /// <param name="targetPos">������תλ��</param>
        public void JumpToAllPosition(int targetAllPos)
        {
            newCard = null;
            //TODO:����֮ǰ�Ŀ���            

            pos = targetAllPos; //�޸�λ��ָ��

            //��ʼ���µĿ���
            newCard = InitCardNode(pos);
            //TODO:���ñ��ֲ�����߼�����
        }

        /// <summary>
        /// ��ʼ����ǰλ�õĿ�����Ϣ(�������ݲ�)
        /// </summary>
        /// <param name="currPos"></param>
        /// <returns>���س�ʼ����Ŀ�����Ϣ</returns>
        private CardNode InitCardNode(int currPos)
        {
            return nodes[currPos];
        }
    }

}