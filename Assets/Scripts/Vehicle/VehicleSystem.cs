using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSystem : MonoBehaviour
{
    // 载具属性：生命值和耐久度
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxDurability = 100f;
    public float currentDurability;

    // 状态
    private enum State { intact, damaged, complete };
    // 当前状态
    private int currentState;
    private int nowState;

    // 载具等级和材料
    public int level = 1;
    public int materials = 0;

    // 背包系统：最大容量、当前容量、物品清单
    public int maxCapacity = 20;
    public int currentCapacity = 0;
    private List<Item> itemList = new List<Item>();

    // 升级花费表：升级需要材料数
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
        // 做处理使得UpdateState受到变化时再调用
        if (nowState != currentState)
        {
            nowState = currentState;
            UpdateState();
        }

        GameOver();
    }

    // 收集物品
    public void CollectItem(Item item)
    {
        if(currentCapacity < maxCapacity)
        {
            itemList.Add(item);
            currentCapacity += item.size;
        }
        else
        {
            Debug.Log("超出容量");
        }
    }

    // 丢弃物品
    public void DiscardItem(Item item)
    {
        itemList.Remove(item);
        currentCapacity -= item.size;
        // 告知
        Debug.Log(item.name);
    }

    // 合成

    // 仓库和背包的交互

    // 载具维修

    // 更新外观
    private void UpdateState()
    {
        // 状态更新
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
        // 分开要清楚一点，最终UI是否显示状态
        switch (currentState)
        {
            case (int)State.intact:
                Debug.Log("完好无损");
                break;
            case (int)State.damaged:
                Debug.Log("部分损伤");
                break;
            case (int)State.complete:
                Debug.Log("完全损伤");
                break;
            default:
                break;
        }
    }

    // 仓库升级

    // 载具升级
    private void UpgradeVehicle()
    {
        if(materials >= upgradeCosts[level] && level < upgradeCosts.Length - 1)
        {
            materials -= upgradeCosts[level];
            level++;
            UpdateVehicleStatus();
        }
    }

    // 更新载具属性
    private void UpdateVehicleStatus()
    {
        maxHealth *= level;
        maxDurability *= level;
        // 速度
    }

    // 碰撞受伤
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

    // 游戏结束
    private void GameOver()
    {
        if (currentHealth == 0)
        {
            // 游戏结束，弹出界面
            Time.timeScale = 0;
        }
    }
}
