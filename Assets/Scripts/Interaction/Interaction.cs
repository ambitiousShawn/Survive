using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Shawn.ProjectFramework;
using System.Runtime.CompilerServices;

public class Interaction : MonoBehaviour
{
    // UI����
    BasePanel panel;

    private bool openDoor = false;
    private bool collect = false;
    private bool destroy = false;

    // ������������Ʒ
    private Collider thing;

    // ����GameObject��ӱ�ǩ,������������trigger Collider����
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
                // ��Ӧ�Ի�����
                DialogueManager.Instance.ShowDialogue(0);
                break;
            default: break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        thing = other;
        // UI�˳��������
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

    // ��Ʒ�ռ��߼�
    private void CollectItem(Collider collider)
    {
        Item item = collider.gameObject.GetComponent<Item>();
        // ��ʾ��E�ռ���***��ʵ��***��

        if (Input.GetKeyDown(KeyCode.E))
        {
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
            Destroy(other.gameObject);

            destroy = false;
        }
    }

    // �Ի�
    private void ShowDialog(int id)
    {

    }
}
