using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shawn.ProjectFramework
{
    [CreateAssetMenu(fileName = "Skill_SO",menuName = "SO/Create Skill")]
    public class SkillData_SO : ScriptableObject
    {
        public List<SkillNode> Nodes;
    }

    [Serializable]
    public class SkillNode
    {
        public int ID;
        public string Name;
        public int Level;
        public string Describtion;
        public int Cost;
        public UnityAction Effect;
    }

}