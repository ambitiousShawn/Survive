using UnityEngine;
using UnityEngine.UI;

namespace Shawn.ProjectFramework
{

    public class UGUI_SettingsPanel : BasePanel
    {
        private Slider mainVolume;
        private Slider musicVolume;
        private Slider soundVolume;
        private Button quitBtn;

        public override void Show()
        {
            base.Show();
            mainVolume = GetControl<Slider>("MainVolume");
            musicVolume = GetControl<Slider>("MusicVolume");
            soundVolume = GetControl<Slider>("SoundVolume");
            quitBtn = GetControl<Button>("QuitBtn");

            quitBtn.onClick.AddListener(() =>
            {
                PanelManager.Instance.HidePanel("UGUI_SettingsPanel");
                PanelManager.Instance.ShowPanel<UGUI_BeginPanel>("UGUI_BeginPanel");
            });

            mainVolume.onValueChanged.AddListener((value) =>
            {
                //添加全局变量模块的音量改变接口逻辑
            });

            musicVolume.onValueChanged.AddListener((value) =>
            {
                //添加全局变量模块的音量改变接口逻辑
            });

            soundVolume.onValueChanged.AddListener((value) =>
            {
                //添加全局变量模块的音量改变接口逻辑
            });
        }
    }

}