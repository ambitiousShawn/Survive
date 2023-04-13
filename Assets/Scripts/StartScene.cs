using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shawn.ProjectFramework;

public class StartScene: MonoBehaviour
{
    void Start()
    {
        PanelManager.Instance.ShowPanel<UGUI_BeginPanel>("UGUI_BeginPanel");
    }
}
