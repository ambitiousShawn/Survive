using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.ProjectFramework
{

    public class GameRoot : MonoBehaviour
    {
        List<IBaseManager> managers = new List<IBaseManager>();

        [SerializeField]
        private bool enableTest = false;

        void Awake()
        {
            managers.Add(new PanelManager());
            managers.Add(new DialogueManager());
            managers.Add(new BuffManager());
            managers.Add(new InventoryManager());
            managers.Add(new TipManager());
            managers.Add(new DrawCardManager());
            managers.Add(new SkillManager());
            managers.Add(new CardProcessManager());

            for (int i = 0;i < managers.Count;i++)
            {
                managers[i]?.Init();
            }
        }

        void Update()
        {
            for (int i = 0;i < managers.Count; i++)
            {
                managers[i]?.Tick();
            }

            ///# Test
            if (enableTest ) Test();
        }

        void Test()
        {
            # region 测试
            if (Input.GetKeyDown(KeyCode.M))
            {
                PanelManager.Instance.ShowPanel<UGUI_BeginPanel>("UGUI_BeginPanel");
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                PanelManager.Instance.ShowPanel<UGUI_MainUIPanel>("UGUI_MainUIPanel");
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                DialogueManager.Instance.ShowDialogue(0);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                DialogueManager.Instance.ShowDialogue(5);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                BuffManager.Instance.AddBuff(0, () =>
                {
                    Debug.Log("和风守护buff已触发");
                });
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                BuffManager.Instance.AddBuff(1, () =>
                {
                    Debug.LogError("烧伤Debuff已触发");
                });
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                BuffManager.Instance.AddBuff(2, () =>
                {
                    Debug.LogWarning("冻伤Debuff已触发");
                });
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                PanelManager.Instance.ShowPanel<UGUI_SkillPanel>("UGUI_SkillPanel");
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                SkillManager.Instance.InitAllSkills();
                SkillManager.Instance.panel.UpdateSkillList();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                InventoryManager.Instance.AddItemToBag(0, 2);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                InventoryManager.Instance.AddItemToBag(1, 3);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                InventoryManager.Instance.AddItemToBag(2, 5);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                InventoryManager.Instance.ConsumeItem("蓝水晶");
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                PanelManager.Instance.ShowPanel<UGUI_WarehousePanel>("UGUI_WarehousePanel");
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                PanelManager.Instance.ShowPanel<UGUI_DrawCardPanel>("UGUI_DrawCardPanel");
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (DrawCardManager.Instance.panel != null)
                {
                    DrawCardManager.Instance.EnableDrawCard();
                }
            }
            # endregion 测试
        }
    }
}