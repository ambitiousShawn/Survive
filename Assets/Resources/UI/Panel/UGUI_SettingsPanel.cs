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
                //���ȫ�ֱ���ģ��������ı�ӿ��߼�
            });

            musicVolume.onValueChanged.AddListener((value) =>
            {
                //���ȫ�ֱ���ģ��������ı�ӿ��߼�
            });

            soundVolume.onValueChanged.AddListener((value) =>
            {
                //���ȫ�ֱ���ģ��������ı�ӿ��߼�
            });
        }
    }

}