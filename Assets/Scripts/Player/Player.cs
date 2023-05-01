using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shawn.ProjectFramework;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    public GameObject CameraPivot;
    UGUI_MainUIPanel panel;

    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.1
    // @brief       ��ɫ����
    //-----------------------------------------------------

    // ����ֵ
    public const float maxHealth = 100f;
    public float currentHealth;
    // ��Һֵ
    public const float maxBodyFluid = 100f;
    public float currentBodyFluid;

    // ÿ��ظ�ֵ
    public float pickUpPerSecond = 0.05f;
    // �˺�
    public float damagePerSecond = 1f;

    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.1
    // @brief       ˤ���˺�����
    //-----------------------------------------------------

    // �߶���ֵ���ڴ�֮�ϲ���ˤ���˺�
    [SerializeField] private float heightThreshold = 5f;
    // ˤ���˺�ϵ��
    [SerializeField] private float fallDamageRatio = 2f;
    // ��¼��߸߶�
    private float maxHeight;

    void Start()
    {
        currentHealth = maxHealth;
        currentBodyFluid = maxBodyFluid;
        rb = GetComponent<Rigidbody>();
    }

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

            // ͬ������Buff����Ʒ���������buffʱ�Ѿ�����
        }

        // ��ɫ����ʱ���ˤ���˺�������
        if (gameObject.GetComponent<PlayerController>().isMoving)
        {
            Die();
            // ��¼��ߵ�
            if(transform.position.y > maxHeight)
            {
                maxHeight = transform.position.y;
                Debug.Log(maxHeight);
            }
        }
    }

    // �����˺�
    public void TakeRatioDamage(float ratio)
    {
        float damage = ratio * maxHealth;
        if (damage > currentHealth)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= ratio * maxHealth;
        }
    }

    // �Խ�ɫ����˺�
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
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
        if (currentBodyFluid > maxBodyFluid)
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

    // debuff�����˺�������ֵ
    public void ContinueReduce(float baseDamage)
    {
        // �����ٶȽ���
        float damagePerSecond = baseDamage * (currentHealth + currentBodyFluid) / (maxHealth + maxBodyFluid);
        StartCoroutine(Continue());
        IEnumerator Continue()
        {
            TakeDamage(damagePerSecond);
            UseRangedAttack(damagePerSecond);

            yield return new WaitForSeconds(1);
        }
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

    // debuff��Ұ���ޣ�����Ϊʱ��
    public void LimitedView(float time)
    {
        StartCoroutine(AddLimited());
        IEnumerator AddLimited()
        {
            // TODO�������Ұ����Ч��
            yield return new WaitForSeconds(time);
            // �������UI
        }
    }

    // ����
    public void Die()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("I am dead");
            // TODO��������������
            Time.timeScale = 0;
        }
    }

    // ��з��֣�ֱ������
    public void GoDie()
    {
        currentHealth = 0;
    }

    // ˤ���˺�
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            float fallDistance = maxHeight - transform.position.y;
            // ������ֵ
            if(fallDistance > heightThreshold)
            {
                float damage = (fallDistance - heightThreshold) * fallDamageRatio;
                TakeDamage(damage);
                // ��غ���߸߶�����
                maxHeight = transform.position.y;
            }
        }
    }
}
