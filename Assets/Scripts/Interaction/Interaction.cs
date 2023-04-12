using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Shawn.ProjectFramework;
using System.Runtime.CompilerServices;

public class Interaction : MonoBehaviour
{
    // UI保存
    BasePanel panel;

    private bool openDoor = false;
    private bool collect = false;
    private bool destroy = false;

    // 如果附近多件物品
    private Collider thing;

    // 所有GameObject添加标签,玩家添加球体检测trigger Collider即可
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Item":
                collect = true;
                break;
            case "Door":
                openDoor = true;
                break;
            case "Destroy":
                destroy = true;
                break;
            case "NPC":
                // 对应对话内容
                DialogueManager.Instance.ShowDialogue(0);
                break;
            default: break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        thing = other;
        // UI退出，待解决
        switch (other.tag)
        {
            case "Item":
            case "Door":
            case "Destroy":
            case "NPC":
                if (panel != null)
                {
                    panel.Hide();
                }
                break;
            default: break;
        }
    }

    private void Update()
    {
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
    }

    // 物品收集逻辑
    private void CollectItem(Collider collider)
    {
        Item item = collider.gameObject.GetComponent<Item>();
        // 显示按E收集（***待实现***）

        if (Input.GetKeyDown(KeyCode.E))
        {
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
            Destroy(other.gameObject);

            destroy = false;
        }
    }

    // 对话
    private void ShowDialog(int id)
    {

    }
}
