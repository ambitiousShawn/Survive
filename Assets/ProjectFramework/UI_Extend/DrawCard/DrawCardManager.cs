using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.ProjectFramework
{
    public class DrawCardManager : IBaseManager
    {
        public static DrawCardManager Instance;

        public enum E_Process
        {
            Idle, Rotating, End
        }

        public static int MinValue = 1;
        public static int MaxValue = 10;

        public Vector3 ExSpeed = new Vector3(0, 0, 800);
        public Vector3 InSpeed = new Vector3(0, 0, -1000);
        public E_Process currProcess = E_Process.Idle;

        public int rand1;
        public int rand2;
        public float waitTime = 1.5f;

        private UGUI_DrawCardPanel panel;
        
        public void Init()
        {
            Instance = this;
        }

        public void Tick()
        {
            if (panel == null)
                panel = PanelManager.Instance.GetPanelByName("UGUI_DrawCardPanel") as UGUI_DrawCardPanel;

            //KeyModelTest();
        }

        //Test
        private void KeyModelTest()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                PanelManager.Instance.ShowPanel<UGUI_DrawCardPanel>("UGUI_DrawCardPanel");
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (panel != null)
                {
                    EnableDrawCard();
                }
            }
        }

        private void EnableDrawCard()
        {
            rand1 = Random.Range(MinValue, MaxValue);
            rand2 = Random.Range(MinValue, MaxValue);
            panel.SingleDraw(rand1,rand2, ExSpeed, InSpeed, waitTime);
        }
    }
}