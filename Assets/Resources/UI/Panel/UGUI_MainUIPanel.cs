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
        /// 更新血条
        /// </summary>
        /// <param name="val">将血条更新至多少</param>
        public void UpdateHealthBar(float val)
        {
            healthBar.fillAmount = val / 100.00f; //TODO:将100换成总血量
        }

        public void UpdateSkillBar(float val)
        {
            skillBar.fillAmount = val / 100.00f; //TODO:将100换成总技能值
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
                //TODO:修改Item的Image贴图组件
                item.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Inventory/背包物品");
            }
        }

        public void UpdateTipInfo(string info)
        {
            notes.text = info;
        }
    }

}