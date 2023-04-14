using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shawn.ProjectFramework
{

    public class UGUI_BasicPanel : BasePanel
    {
        private Button btn_Refrigerator;
        private Button btn_TV;
        private Button btn_Funs;
        private Button btn_Closet;

        public override void Show()
        {
            base.Show();

            btn_Refrigerator = GetControl<Button>("Btn_Refrigerator");
            btn_TV = GetControl<Button>("Btn_TV");
            btn_Funs = GetControl<Button>("Btn_Funs");
            btn_Closet = GetControl<Button>("Btn_Closet");

            btn_Refrigerator.onClick.AddListener(() =>
            {
                //TODO:
            });
            btn_TV.onClick.AddListener(() =>
            {
                //TODO:
            });
            btn_Funs.onClick.AddListener(() =>
            {
                //TODO:
            });
            btn_Closet.onClick.AddListener(() =>
            {
                //TODO:
            });
        }
    }

}