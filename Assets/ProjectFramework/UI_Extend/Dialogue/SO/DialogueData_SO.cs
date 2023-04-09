using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.EditorFramework
{
    /// <summary>
    /// �Ի��Ŀ��ӻ�����SO
    /// </summary>
    [CreateAssetMenu(fileName = "Dialogue Data", menuName = "SO/Create Dialogue")]
    public class DialogueData_SO : ScriptableObject
    {
        public List<DialogueNode> Nodes;
    }

    /// <summary>
    /// �Ի�����ö��
    /// </summary>
    public enum DialogueNodeType
    {
        Text, //���ı�
        TextWithAvatar, //�ı���ͷ��
        Option, //��ѡ��
        CustomEvent
    }

    /// <summary>
    /// �Ի��ڵ�
    /// </summary>
    [Serializable]
    public class DialogueNode
    {
        public int Pos; //�Ի�λ��
        public DialogueNodeType Type; //�Ի�����
        public Sprite Avatar; //�Ի�ͷ��
        [TextArea]
        public string Info; //�Ի�����
        public List<DialogueOption> Options; //ѡ��
        public List<DialogueEvent> Events; //�Ի���Ļص�
    }

    [Serializable]
    public class DialogueOption
    {
        public string Info; //ѡ�������
        public int Pos; //��һ���Ի��ڵ�ı��
    }

    [Serializable]
    public class DialogueEvent
    {
        public string EventName;
        public string Value;
    }
}