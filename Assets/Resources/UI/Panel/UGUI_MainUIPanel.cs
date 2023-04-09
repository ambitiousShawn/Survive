using Shawn.ProjectFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Shawn.ProjectFramework
{

    public class UGUI_MainUIPanel : BasePanel
    {
        private const string BUFF_PATH = "UI/Element/Buff";
        private const string ITEM_PATH = "UI/Element/Item";
        private const string BUFF_TEXTURE_PATH = "Art/Buff/";

        private Image healthBar;
        private Image skillBar;
        private Text notes;
        private Transform buffFrame;
        private Transform itemFrame;

        public override void Show()
        {
            base.Show();

            healthBar = GetControl<Image>("HealthBar");
            skillBar = GetControl<Image>("SkillBar");
            notes = GetControl<Text>("Bird");
            buffFrame = transform.Find("LeftTop/BuffFrame");
            itemFrame = transform.Find("Bottom/Bag/Grids");
        }

        /// <summary>
        /// ����Ѫ��
        /// </summary>
        /// <param name="val">��Ѫ������������</param>
        public void UpdateHealthBar(float val)
        {
            healthBar.fillAmount = val / 100.00f; //TODO:��100������Ѫ��
        }

        public void UpdateSkillBar(float val)
        {
            skillBar.fillAmount = val / 100.00f; //TODO:��100�����ܼ���ֵ
        }

        public void UpdateBuffFrame()
        {
            GameObject buff;
            for (int i = 0; i < buffFrame.childCount; i++)
            {
                Destroy(buffFrame.GetChild(i).gameObject);
            }

            foreach (string name in BuffManager.Instance.buffDic.Keys)
            {
                buff = ResourcesManager.Instance.Load<GameObject>(BUFF_PATH);
                buff.transform.SetParent(buffFrame);
                buff.name = name;
                buff.GetComponentInChildren<Text>().text = buff.name;
                buff.transform.localScale = Vector3.one;
                buff.GetComponent<Image>().sprite = Resources.Load<Sprite>(BUFF_TEXTURE_PATH + name);
            }
        }

        public void UpdateInventoryItem()
        {
            GameObject item;
            for (int i = 0; i < itemFrame.childCount; i++)
            {
                Destroy(itemFrame.GetChild(i).gameObject);
            }

            foreach (string name in InventoryManager.Instance.ItemDicFromBag.Keys)
            {
                item = ResourcesManager.Instance.Load<GameObject>(ITEM_PATH);
                item.transform.SetParent(itemFrame);
                item.name = name;
                item.transform.localScale = Vector3.one;
                item.GetComponentInChildren<Text>().text = InventoryManager.Instance.ItemNum[name].ToString();
                //TODO:�޸�Item��Image��ͼ���
                item.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Inventory/������Ʒ");
            }
        }

        public void UpdateTipInfo(string info)
        {
            notes.text = info;
        }
    }

}