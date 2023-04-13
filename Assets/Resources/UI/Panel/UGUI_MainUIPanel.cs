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
        private const string PICKUP_PATH = "UI/Element/Pickup";

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


        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="val"></param>
        public void UpdateSkillBar(float val)
        {
            skillBar.fillAmount = val / 100.00f; //TODO:��100�����ܼ���ֵ
        }

        /// <summary>
        /// ����Buff�б�
        /// </summary>
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

        /// <summary>
        /// ���²ֿ���Ʒ
        /// </summary>
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

        /// <summary>
        /// ������ʾ����
        /// </summary>
        /// <param name="info"></param>
        public void UpdateTipInfo(string info)
        {
            notes.text = info;
        }

        private GameObject pick;

        /// <summary>
        /// ��ʾʰȡͼ��
        /// </summary>
        /// <param name="itemPos"></param>
        public void ShowPickUp(Vector3 itemPos, string info ,int type = 1)
        {
            if (pick != null) return;
            Vector3 uiPos = Camera.main.WorldToScreenPoint(itemPos);
            pick = ResourcesManager.Instance.Load<GameObject>(PICKUP_PATH + type.ToString());
            pick.transform.parent = transform;
            pick.transform.localScale = Vector3.one;
            pick.transform.localPosition = uiPos;
            pick.GetComponentInChildren<Text>().text = info;
        }

        public void HidePickUp()
        {
            if (pick == null) return;
            Destroy(pick);
            pick = null;
        }
    }

}