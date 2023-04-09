using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shawn.ProjectFramework
{

    [CreateAssetMenu(fileName = "Buff_SO", menuName = "SO/Create Buff")]
    public class BuffData_SO : ScriptableObject
    {
        public List<BuffNode> Nodes;
    }

    public enum E_BuffType
    {
        Buff,
        Debuff
    }

    [Serializable]
    public class BuffNode
    {
        public int ID;
        public E_BuffType Type;
        public string Name;
        public string Describtion;
        public UnityAction Callback;
        public float Time;
        public float Interval;
    }
}