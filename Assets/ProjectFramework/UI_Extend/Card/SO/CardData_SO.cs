using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shawn.ProjectFramework
{

    [CreateAssetMenu(fileName = "Card_SO", menuName = "SO/Create Card")]
    public class CardData_SO : ScriptableObject
    {
        public List<CardNode> Nodes;
    }

    public enum E_Card_Type
    {
        Environment,
        Collection,
        Event
    }

    [Serializable]
    public class CardNode
    {
        public int ID;
        public E_Card_Type Type;
        public string Name;
        public string Describtion;
        public UnityAction Effect;
        public List<NextInfo> NextList;
    }

    [Serializable]
    public class NextInfo
    {
        public int subID;
        public string Text;
        public int TargetID;
    }
}