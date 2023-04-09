using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.ProjectFramework
{

    [CreateAssetMenu(fileName = "Tip_SO", menuName = "SO/Create Tip")]
    public class TipData_SO : ScriptableObject
    {
        public List<TipNode> Nodes;
    }

    [Serializable]
    public class TipNode
    {
        public int ID;
        public string Q;
        public string A;
    }

}