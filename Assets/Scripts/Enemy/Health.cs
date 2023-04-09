using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // 最大生命值
    public float maxHealth = 100f;
    // 当前血量
    public float currentHealth;
    // 体液值(方法)
    public float maxBodyFluid = 100f;
    public float currentBodyFluid;

    private void Start()
    {
        // 初始化当前血量为最大血量
        currentHealth = maxHealth;
        // 初始化体液值
        currentBodyFluid = maxBodyFluid;
    }

    // 受到伤害的方法
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0)
        {
            Die();
        }
    }

    // 使用体液攻击
    public void UseAttack(float usage)
    {
        currentBodyFluid -= usage;
    }

    // 死亡
    public void Die()
    {
        Debug.Log("I am dead");
        Destroy(gameObject);
    }
}
