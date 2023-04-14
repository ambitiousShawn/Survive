using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Shawn.ProjectFramework;
using System.Runtime.CompilerServices;

public class Interaction : MonoBehaviour
{
    UGUI_MainUIPanel panel; 

    private bool openDoor = false;
    private bool collect = false;
    private bool destroy = false;
    private bool talk = false;

    // 如果附近多件物品
    private Collider thing;

    // 所有GameObject添加标签,玩家添加球体检测trigger Collider即可
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Item":
                thing = other;
                collect = true;
                panel?.ShowPickUp(other.transform.position, "拾取"); //显示交互
                break;
            case "Door":
                thing = other;
                openDoor = true;
                panel?.ShowPickUp(other.transform.position, "开启"); //显示交互
                break;
            case "Destroy":
                thing = other;
                destroy = true;
                panel?.ShowPickUp(other.transform.position, "摧毁"); //显示交互
                break;
            case "NPC":
                thing = other;
                talk = true;
                panel?.ShowPickUp(other.transform.position, "交流"); //显示交互
                break;
            default: break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        switch (other.tag)
        {
            case "Item":
                collect = false;
                panel?.HidePickUp();
                break;
            case "Door":
                openDoor= false;
                panel?.HidePickUp();
                break;
            case "Destroy":
                destroy = false;
                panel?.HidePickUp();
                break;
            case "NPC":
                talk = false;
                panel?.HidePickUp();
                break;
            default: break;
        }
    }

    private void Update()
    {
        if (panel == null) 
        {
            panel = PanelManager.Instance.GetPanelByName("UGUI_MainUIPanel") as UGUI_MainUIPanel;
        }
        if (collect)
        {
            CollectItem(thing);
        }
        else if (destroy)
        {
            DestroyItem(thing);
        }
        else if (openDoor)
        {
            OpenDoor(thing);
        }
        else if (talk)
        {
            //TODO:后续更改具体对话
            if (Input.GetKeyDown(KeyCode.E))
            {
                panel?.HidePickUp();
                DialogueManager.Instance.ShowDialogue(0);
            }
            
        }
    }

    // 物品收集逻辑
    private void CollectItem(Collider collider)
    {
        Item item = collider.gameObject.GetComponent<Item>();
        // 显示按E收集（***待实现***）

        if (Input.GetKeyDown(KeyCode.E) && item != null)
        {
            panel?.HidePickUp();
            // ID如何获取
            int id = item.ID;
            InventoryManager.Instance.AddItemToBag(id);
            Destroy(collider.gameObject);

            collect = false;
        }
    }

    // 开门逻辑
    private void OpenDoor(Collider other)
    {
        // 显示按E开门

        if (Input.GetKeyDown(KeyCode.E))
        {
            panel?.HidePickUp();
            // 具体怎么移动或者消失方式，信息可以在脚本中实现
            Destroy(other.gameObject);

            openDoor = false;
        }
    }

    // 场景破坏
    private void DestroyItem(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            panel?.HidePickUp();
            Destroy(other.gameObject);

            destroy = false;
        }
    }
}
