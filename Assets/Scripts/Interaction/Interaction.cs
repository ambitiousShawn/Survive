using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Shawn.ProjectFramework;
using System.Runtime.CompilerServices;

public class Interaction : MonoBehaviour
{
    // UI����
    BasePanel panel; // TODO����Ҫʵ����ʾ����E���ռ������ţ��ƻ����Ի�

    private bool openDoor = false;
    private bool collect = false;
    private bool destroy = false;

    // ������������Ʒ
    private Collider thing;

    // ����GameObject��ӱ�ǩ,������������trigger Collider����
    private void OnTriggerEnter(Collider other)
    {
        thing = other;
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
        // UI�˳��������
        // TODO��ʵ����ʾ����Ч��
        switch (other.tag)
        {
            case "Item":
                collect = false;
                break;
            case "Door":
                openDoor= false;
                break;
            case "Destroy":
                destroy = false;
                break;
            case "NPC":
                break;
            default: break;
        }
        if (panel != null)
        {
            panel.Hide();
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

        if (Input.GetKeyDown(KeyCode.E) && item != null)
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
