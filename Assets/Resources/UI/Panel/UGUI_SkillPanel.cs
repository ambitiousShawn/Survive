using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shawn.ProjectFramework
{

    public class UGUI_SkillPanel : BasePanel
    {
        private Transform skills;
        private const string SKILL_PATH = "UI/Element/Skill";

        private Button btn_Exit;

        public override void Show()
        {
            base.Show();
            skills = transform.Find("Frame/ScrollView/Viewport/Skills");
            btn_Exit = GetControl<Button>("Exit");

            btn_Exit.onClick.AddListener(() =>
            {
                PanelManager.Instance.HidePanel("UGUI_SkillPanel");
            });
        }

        public void UpdateSkillList()
        {
            GameObject item;
            for (int i = 0; i < skills.childCount; i++)
            {
                Destroy(skills.GetChild(i).gameObject);
            }

            foreach (string name in SkillManager.Instance.currSkillDic.Keys)
            {
                item = ResourcesManager.Instance.Load<GameObject>(SKILL_PATH);
                item.transform.SetParent(skills);
                item.name = name;
                item.transform.localScale = Vector3.one;
                //TODO:修改Item的Image贴图组件
            }
        }
    }
}