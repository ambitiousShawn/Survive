using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.ProjectFramework
{

    public class GameRoot : MonoBehaviour
    {
        List<IBaseManager> managers = new List<IBaseManager>();

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
        }
    }
}