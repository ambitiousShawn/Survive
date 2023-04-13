using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shawn.ProjectFramework
{
    public class UGUI_Item : MonoBehaviour
    {
        const string BAG_PARENT_NAME = "Grids";
        const string House_PARENT_NAME = "Items";
        private Button Btn;
        private UGUI_WarehousePanel panel;

        private void Start()
        {
            
            Btn = GetComponent<Button>();
            Btn.onClick.AddListener(() =>
            {
                if (panel == null) return;
                string name = gameObject.name;
                int id;
                if (transform.parent.name == BAG_PARENT_NAME)
                {
                    id = InventoryManager.Instance.ItemDicFromBag[name].ID;
                    panel.UpdateInfoFrame(id);
                    InventoryManager.Instance.TransferItem(id, InventoryManager.E_TransferType.FromBagToHouse);
                }else if (transform.parent.name == House_PARENT_NAME)
                {
                    id = InventoryManager.Instance.ItemDicFromHouse[name].ID;
                    panel.UpdateInfoFrame(id);
                    InventoryManager.Instance.TransferItem(id, InventoryManager.E_TransferType.FromHouseToBag);
                }
            });
            
        }

        private void Update()
        {
            if (panel == null) panel = PanelManager.Instance.GetPanelByName("UGUI_WarehousePanel") as UGUI_WarehousePanel;
        }
    }

}