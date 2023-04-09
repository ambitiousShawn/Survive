using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
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

    // �ܵ��˺��ķ���
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0)
        {
            Die();
        }
    }

    // ʹ����Һ����
    public void UseAttack(float usage)
    {
        currentBodyFluid -= usage;
    }

    // ����
    public void Die()
    {
        Debug.Log("I am dead");
        Destroy(gameObject);
    }
}
