using Shawn.ProjectFramework;
using UnityEngine.UI;

namespace Shawn.ProjectFramework
{
    public class UGUI_DialoguePanel : BasePanel
    {
        private Text info;

        public override void Init()
        {
            base.Init();

            info = GetControl<Text>("Info");
        }

        public override void Show()
        {
            base.Show();

            UpdateInfo(DialogueManager.Instance.tempList[DialogueManager.Instance.pos].Info);
        }

        public void UpdateInfo(string textInfo)
        {
            info.text = textInfo;
        }

        public void AppendInfo(char appendChar)
        {
            info.text += appendChar;
        }
    }

}