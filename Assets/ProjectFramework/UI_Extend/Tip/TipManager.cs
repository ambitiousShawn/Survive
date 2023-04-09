using Shawn.EditorFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.ProjectFramework
{

    public class TipManager : IBaseManager
    {
        private const string TIP_PATH = "SO/Tip_SO";
        public static TipManager Instance;

        private List<TipNode> tipList;
        private UGUI_TipPanel panel;

        public void Init()
        {
            Instance = this;

            tipList = Resources.Load<TipData_SO>(TIP_PATH).Nodes;
        }

        public void Tick()
        {
            if (panel == null) panel = PanelManager.Instance.GetPanelByName("UGUI_TipPanel") as UGUI_TipPanel;
            if (panel != null && Input.GetKeyDown(KeyCode.Escape))
            {
                HideTip();
            }
        }

        public void ShowRandomTip()
        {
            if (panel == null) panel = PanelManager.Instance.GetPanelByName("UGUI_TipPanel") as UGUI_TipPanel;
            PanelManager.Instance.ShowPanel<UGUI_TipPanel>("UGUI_TipPanel");
            int rand = Random.Range(0, tipList.Count);
            if (panel != null) panel.UpdateQAndA(tipList[rand].Q, tipList[rand].A);
        }

        public void HideTip()
        {
            panel = null;
            PanelManager.Instance.HidePanel("UGUI_TipPanel");
        }
    }
}