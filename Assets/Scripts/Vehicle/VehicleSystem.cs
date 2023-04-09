using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSystem : MonoBehaviour
{
    // �ؾ����ԣ�����ֵ���;ö�
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxDurability = 100f;
    public float currentDurability;

    // ״̬
    private enum State { intact, damaged, complete };
    // ��ǰ״̬
    private int currentState;
    private int nowState;

    // �ؾߵȼ��Ͳ���
    public int level = 1;
    public int materials = 0;

    // ����ϵͳ�������������ǰ��������Ʒ�嵥
    public int maxCapacity = 20;
    public int currentCapacity = 0;
    private List<Item> itemList = new List<Item>();

    // �������ѱ�������Ҫ������
    private readonly int[] upgradeCosts = { 0, 10, 20, 40, 80 };

    private void Start()
    {
        currentHealth = maxHealth;
        currentDurability = maxDurability;
        currentState = (int)State.intact;
        nowState = currentState;
    }

    private void Update()
    {
        // ������ʹ��UpdateState�ܵ��仯ʱ�ٵ���
        if (nowState != currentState)
        {
            nowState = currentState;
            UpdateState();
        }

        GameOver();
    }

    // �ռ���Ʒ
    public void CollectItem(Item item)
    {
        if(currentCapacity < maxCapacity)
        {
            itemList.Add(item);
            currentCapacity += item.size;
        }
        else
        {
            Debug.Log("��������");
        }
    }

    // ������Ʒ
    public void DiscardItem(Item item)
    {
        itemList.Remove(item);
        currentCapacity -= item.size;
        // ��֪
        Debug.Log(item.name);
    }

    // �ϳ�

    // �ֿ�ͱ����Ľ���

    // �ؾ�ά��

    // �������
    private void UpdateState()
    {
        // ״̬����
        if (currentDurability == maxDurability)
        {
            currentState = (int)State.intact;
        }
        else if (currentDurability == 0)
        {
            currentState = (int)State.complete;
        }
        else
        {
            currentState = (int)State.damaged;
        }
        // �ֿ�Ҫ���һ�㣬����UI�Ƿ���ʾ״̬
        switch (currentState)
        {
            case (int)State.intact:
                Debug.Log("�������");
                break;
            case (int)State.damaged:
                Debug.Log("��������");
                break;
            case (int)State.complete:
                Debug.Log("��ȫ����");
                break;
            default:
                break;
        }
    }

    // �ֿ�����

    // �ؾ�����
    private void UpgradeVehicle()
    {
        if(materials >= upgradeCosts[level] && level < upgradeCosts.Length - 1)
        {
            materials -= upgradeCosts[level];
            level++;
            UpdateVehicleStatus();
        }
    }

    // �����ؾ�����
    private void UpdateVehicleStatus()
    {
        maxHealth *= level;
        maxDurability *= level;
        // �ٶ�
    }

    // ��ײ����
    public void Damage(float damage)
    {
        if(currentHealth > damage)
        {
            currentHealth -= damage;
        }
        else
        {
            currentHealth = 0;
        }
    }

    // ��Ϸ����
    private void GameOver()
    {
        if (currentHealth == 0)
        {
            // ��Ϸ��������������
            Time.timeScale = 0;
        }
    }
}
