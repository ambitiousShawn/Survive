using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private EnemyUI enemyUI;      // ���� UI ���

    // �������ֵ
    public float maxHealth = 100f;
    // ��ǰѪ��
    public float currentHealth;
    // ��Һֵ(����)
    public float maxBodyFluid = 100f;
    public float currentBodyFluid;

    private void Start()
    {
        // ��ʼ����ǰѪ��Ϊ���Ѫ��
        currentHealth = maxHealth;
        // ��ʼ����Һֵ
        currentBodyFluid = maxBodyFluid;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // �ܵ��˺��ķ���
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

    // ʹ����Һ����
    public void UseAttack(float usage)
    {
        currentBodyFluid -= usage;
    }

    // ��Ѫ
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

    // ����
    public void Die()
    {
        Debug.Log("Enemy is dead");
        Destroy(gameObject);
        Destroy(enemyUI.gameObject);
    }
}
