using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shawn.ProjectFramework
{

    public class UGUI_WarehousePanel : BasePanel
    {
        private const string ITEM_PATH = "UI/Element/Item";
        
        private Transform itemFrame;
        private Text Name;
        private Image image;
        private Text Describtion;
        private Button Btn_Exit;

        public override void Show()
        {
            base.Show();
            itemFrame = transform.Find("Frame/Items");
            Name = GetControl<Text>("Name");
            Describtion = GetControl<Text>("Describtion");
            image = GetControl<Image>("Image");
            Btn_Exit = GetControl<Button>("Exit");
            Btn_Exit.onClick.AddListener(() =>
            {
                PanelManager.Instance.HidePanel("UGUI_WarehousePanel");
            });
        }

        /// <summary>
        /// 更新右侧物品信息栏
        /// </summary>
        /// <param name="id"></param>
        public void UpdateInfoFrame(int id)
        {
            ItemNode item = InventoryManager.Instance.itemList[id];
            Name.text = item.Name;
            Describtion.text = item.Describtion;
            //image.sprite = Resources.Load<Sprite>("");
            
        }

        public void UpdateHouseItem()
        {
            GameObject item;
            for (int i = 0; i < itemFrame.childCount; i++)
            {
                Destroy(itemFrame.GetChild(i).gameObject);
            }

            foreach (string name in InventoryManager.Instance.ItemDicFromHouse.Keys)
            {
                item = ResourcesManager.Instance.Load<GameObject>(ITEM_PATH);
                item.transform.SetParent(itemFrame);
                item.name = name;
                item.transform.localScale = Vector3.one;
                item.GetComponentInChildren<Text>().text = InventoryManager.Instance.ItemNum[name].ToString();
                //TODO:修改Item的Image贴图组件
                item.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Inventory/仓库物品");
            }
        }
    }

}