using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shawn.ProjectFramework
{

    public class UGUI_TipPanel : BasePanel
    {
        private Text Q;
        private Text A;

        public override void Init()
        {
            base.Init();
            Q = GetControl<Text>("Question");
            A = GetControl<Text>("Answer");
        }

        public void UpdateQAndA(string q, string a)
        {
            Q.text = q;
            A.text = a;
        }
    }

}