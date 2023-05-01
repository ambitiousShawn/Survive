using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Attack
{
    [CreateAssetMenu(fileName = "Attack_SO", menuName = "SO/Create Attack")]
    public class AttackData_SO : ScriptableObject
    {
        public List<AttackData> root;
    }

    [Serializable]
    public class AttackData
    {
        public SkillState skillState;
        public float damageRatio;
        public float fluidConsume;
        public float duration;
        public string describtion;
        // TODO：debuff唯一的情况
        public UnityAction doEffect;
    }
}
