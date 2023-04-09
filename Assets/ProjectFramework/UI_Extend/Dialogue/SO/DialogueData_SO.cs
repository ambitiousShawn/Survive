using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.EditorFramework
{
    /// <summary>
    /// 对话的可视化数据SO
    /// </summary>
    [CreateAssetMenu(fileName = "Dialogue Data", menuName = "SO/Create Dialogue")]
    public class DialogueData_SO : ScriptableObject
    {
        public List<DialogueNode> Nodes;
    }

    /// <summary>
    /// 对话类型枚举
    /// </summary>
    public enum DialogueNodeType
    {
        Text, //纯文本
        TextWithAvatar, //文本带头像
        Option, //带选项
        CustomEvent
    }

    /// <summary>
    /// 对话节点
    /// </summary>
    [Serializable]
    public class DialogueNode
    {
        public int Pos; //对话位置
        public DialogueNodeType Type; //对话类型
        public Sprite Avatar; //对话头像
        [TextArea]
        public string Info; //对话内容
        public List<DialogueOption> Options; //选项
        public List<DialogueEvent> Events; //对话后的回调
    }

    [Serializable]
    public class DialogueOption
    {
        public string Info; //选项的内容
        public int Pos; //下一条对话节点的编号
    }

    [Serializable]
    public class DialogueEvent
    {
        public string EventName;
        public string Value;
    }
}