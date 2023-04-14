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

    // ������������Ʒ
    private Collider thing;

    // ����GameObject��ӱ�ǩ,������������trigger Collider����
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Item":
                thing = other;
                collect = true;
                panel?.ShowPickUp(other.transform.position, "ʰȡ"); //��ʾ����
                break;
            case "Door":
                thing = other;
                openDoor = true;
                panel?.ShowPickUp(other.transform.position, "����"); //��ʾ����
                break;
            case "Destroy":
                thing = other;
                destroy = true;
                panel?.ShowPickUp(other.transform.position, "�ݻ�"); //��ʾ����
                break;
            case "NPC":
                thing = other;
                talk = true;
                panel?.ShowPickUp(other.transform.position, "����"); //��ʾ����
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
            //TODO:�������ľ���Ի�
            if (Input.GetKeyDown(KeyCode.E))
            {
                panel?.HidePickUp();
                DialogueManager.Instance.ShowDialogue(0);
            }
            
        }
    }

    // ��Ʒ�ռ��߼�
    private void CollectItem(Collider collider)
    {
        Item item = collider.gameObject.GetComponent<Item>();
        // ��ʾ��E�ռ���***��ʵ��***��

        if (Input.GetKeyDown(KeyCode.E) && item != null)
        {
            panel?.HidePickUp();
            // ID��λ�ȡ
            int id = item.ID;
            InventoryManager.Instance.AddItemToBag(id);
            Destroy(collider.gameObject);

            collect = false;
        }
    }

    // �����߼�
    private void OpenDoor(Collider other)
    {
        // ��ʾ��E����

        if (Input.GetKeyDown(KeyCode.E))
        {
            panel?.HidePickUp();
            // ������ô�ƶ�������ʧ��ʽ����Ϣ�����ڽű���ʵ��
            Destroy(other.gameObject);

            openDoor = false;
        }
    }

    // �����ƻ�
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
