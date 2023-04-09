using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shawn.ProjectFramework
{

    public class UGUI_Skill : MonoBehaviour
    {
        public Image tex;
        public Image backframe;
        public Text nameText;
        public Transform processToggles;
        public Text describtion;
        public Text cost;
        public Button levelUp;

        private int maxLevel = 5;
        private int currLevel ;
        private SkillNode node;

        void Start()
        {
            string name = gameObject.name;
            //TODO:更换贴图
            nameText.text = name;
            node = SkillManager.Instance.currSkillDic[name];
            currLevel = 0;
            describtion.text = node.Describtion;
            cost.text = node.Cost.ToString();
            tex.sprite = Resources.Load<Sprite>("Art/Skill" + name);
            tex.SetNativeSize();
            backframe.sprite = Resources.Load<Sprite>("Art/Skill/f_" + name);
            levelUp.onClick.AddListener(() =>
            {
                if (currLevel < maxLevel)
                {
                    currLevel++;
                    UpdateSkillLevel();
                }
            });
        }

        //技能升级
        private void UpdateSkillLevel()
        {
            for (int i = 0;i < currLevel; i++)
            {
                Toggle level = processToggles.GetChild(i).GetComponent<Toggle>();
                level.isOn = true;
            }
        }
        
    }

}