using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shawn.ProjectFramework;

public class Player : MonoBehaviour
{
    Rigidbody rb;

    // ����ֵ
    public const float maxHealth = 100f;
    public float currentHealth;
    // ��Һֵ
    public const float maxBodyFluid = 100f;
    public float currentBodyFluid;
    // ÿ��ظ�ֵ
    public float pickUpPerSecond = 0.05f;

    private float damage = 2f;
    UGUI_MainUIPanel panel;

    // �߶ȣ��ڴ�֮�ϲ���ˤ���˺�
    [SerializeField] private float height = 5f;
    [SerializeField] private float fallDamageRatio = 5f;

    private void Update()
    {
        // �����������UI��������ֵ����Һֵ��Buff
        if (panel == null)
        {
            panel = PanelManager.Instance.GetPanelByName("UGUI_MainUIPanel") as UGUI_MainUIPanel;
        }
        else
        {
            panel.UpdateHealthBar(currentHealth);
            panel.UpdateSkillBar(currentBodyFluid);

            // ͬ������Buff����Ʒ��
        }

        FallDamage();
        Die();
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentBodyFluid = maxBodyFluid;
        rb = GetComponent<Rigidbody>();
    }

    // ����ֵ����
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    // ˤ���˺�
    private void FallDamage()
    {
        // �����ٶ���ȷ���˺�
        float criticalVelocity = Mathf.Sqrt(-2 * height * Physics.gravity.y);
        bool touchGround = gameObject.GetComponent<PlayerController>().isGrounded;
        float fallDamage = (Mathf.Pow(rb.velocity.y, 2) / -2 / Physics.gravity.y - height) * fallDamageRatio;
        // ���
        if (rb.velocity.y < -criticalVelocity && touchGround)
        {
            bool trigger = true;
            if (trigger) 
            {
                // Э��
                StartCoroutine(Fall());
                IEnumerator Fall()
                {
                    TakeDamage(fallDamage);
                    trigger = false;
                    Debug.Log("FallDamage!");

                    yield return new WaitForSeconds(1);
                }
            }
        }
    }

    // ��Һֵ����
    public void UseRangedAttack(float usage)
    {
        currentBodyFluid -= usage;
    }

    // ����ֵ��Ʒ����
    public void HealthPickUp(float health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // ��Һֵ��Ʒ����
    public void BodyFluidPickUp(float fluid)
    {
        currentBodyFluid += fluid;
        if(currentBodyFluid > maxBodyFluid)
        {
            currentBodyFluid = maxBodyFluid;
        }
    }

    // ����ݻظ�ϵͳ
    public void PickUp()
    {
        // �ָ��ٶ��Ƿ�һ��
        HealthPickUp(pickUpPerSecond);
        BodyFluidPickUp(pickUpPerSecond * 2);
    }

    // debuffѣ��
    private void Vertigo()
    {
        
    }

    // debuff��Ұ����
    private void LimitedView()
    {

    }

    // debuff�����˺�������ֵ
    private void ContinueDamage()
    {
        float damagePerSecond = 1f;
        StartCoroutine(Continue());
        IEnumerator Continue()
        {
            TakeDamage(damagePerSecond);
            UseRangedAttack(damagePerSecond);

            yield return new WaitForSeconds(1);
        }
    }

    // ����
    public void Die()
    {
        if (currentHealth < 0)
        {
            Debug.Log("I am dead");
            Time.timeScale = 0;
        }
    }
}
