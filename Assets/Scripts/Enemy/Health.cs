using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private EnemyUI enemyUI;      // 敌人 UI 组件

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

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 受到伤害的方法
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }
        else
        {
            enemyUI.UpdateHealth(currentHealth);
        }
    }

    // 使用体液攻击
    public void UseAttack(float usage)
    {
        currentBodyFluid -= usage;
    }

    // 流血
    public void ContinueDamage(float damagePerSecond, float duration)
    {
        StartCoroutine(Continue());
        IEnumerator Continue()
        {
            float timer = 0f;
            while (timer < duration)
            {
                TakeDamage(damagePerSecond * Time.deltaTime);

                yield return null;

                timer += Time.deltaTime;
            }
        }
    }

    // 死亡
    public void Die()
    {
        Debug.Log("Enemy is dead");
        Destroy(gameObject);
        Destroy(enemyUI.gameObject);
    }
}
