using Shawn.ProjectFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Shawn.ProjectFramework
{

    public class UGUI_BeginPanel : BasePanel
    {
        private Button startBtn;
        private Button loadBtn;
        private Button settingsBtn;
        private Button quitBtn;

        public override void Show()
        {
            base.Show();

            startBtn = GetControl<Button>("StartBtn");
            loadBtn = GetControl<Button>("LoadBtn");
            settingsBtn = GetControl<Button>("SettingsBtn");
            quitBtn = GetControl<Button>("QuitBtn");

            startBtn.onClick.AddListener(() =>
            {
                //TODO:添加切换场景到游戏的接口逻辑
                PanelManager.Instance.HidePanel("UGUI_BeginPanel");
                PanelManager.Instance.ShowPanel<LoadUI>("LoadUI");
            });

            loadBtn.onClick.AddListener(() =>
            {
                //TODO:添加载入游戏场景到游戏的接口逻辑
            });

            settingsBtn.onClick.AddListener(() =>
            {
                PanelManager.Instance.ShowPanel<UGUI_SettingsPanel>("UGUI_SettingsPanel");
                PanelManager.Instance.HidePanel("UGUI_BeginPanel");
            });

            quitBtn.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }
    }

}