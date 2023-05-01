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
    // @brief       角色属性
    //-----------------------------------------------------

    // 生命值
    public const float maxHealth = 100f;
    public float currentHealth;
    // 体液值
    public const float maxBodyFluid = 100f;
    public float currentBodyFluid;

    // 每秒回复值
    public float pickUpPerSecond = 0.05f;
    // 伤害
    public float damagePerSecond = 1f;

    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.1
    // @brief       摔落伤害数据
    //-----------------------------------------------------

    // 高度阈值，在此之上产生摔落伤害
    [SerializeField] private float heightThreshold = 5f;
    // 摔落伤害系数
    [SerializeField] private float fallDamageRatio = 2f;
    // 记录最高高度
    private float maxHeight;

    void Start()
    {
        currentHealth = maxHealth;
        currentBodyFluid = maxBodyFluid;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 在这里调用主UI更新生命值、体液值和Buff
        if (panel == null)
        {
            panel = PanelManager.Instance.GetPanelByName("UGUI_MainUIPanel") as UGUI_MainUIPanel;
        }
        else
        {
            panel.UpdateHealthBar(currentHealth);
            panel.UpdateSkillBar(currentBodyFluid);

            // 同样更新Buff和物品栏，在添加buff时已经更新
        }

        // 角色控制时检测摔落伤害和死亡
        if (gameObject.GetComponent<PlayerController>().isMoving)
        {
            Die();
            // 记录最高点
            if(transform.position.y > maxHeight)
            {
                maxHeight = transform.position.y;
                Debug.Log(maxHeight);
            }
        }
    }

    // 比例伤害
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

    // 对角色造成伤害
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
    // 体液值降低
    public void UseRangedAttack(float usage)
    {
        currentBodyFluid -= usage;
    }

    // 生命值物品回升
    public void HealthPickUp(float health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    // 体液值物品回升
    public void BodyFluidPickUp(float fluid)
    {
        currentBodyFluid += fluid;
        if (currentBodyFluid > maxBodyFluid)
        {
            currentBodyFluid = maxBodyFluid;
        }
    }

    // 风滚草回复系统
    public void PickUp()
    {
        // 恢复速度是否一致
        HealthPickUp(pickUpPerSecond);
        BodyFluidPickUp(pickUpPerSecond * 2);
    }

    // debuff持续伤害，忍耐值
    public void ContinueReduce(float baseDamage)
    {
        // 降低速度降慢
        float damagePerSecond = baseDamage * (currentHealth + currentBodyFluid) / (maxHealth + maxBodyFluid);
        StartCoroutine(Continue());
        IEnumerator Continue()
        {
            TakeDamage(damagePerSecond);
            UseRangedAttack(damagePerSecond);

            yield return new WaitForSeconds(1);
        }
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

    // debuff视野受限，参数为时长
    public void LimitedView(float time)
    {
        StartCoroutine(AddLimited());
        IEnumerator AddLimited()
        {
            // TODO：添加视野受限效果
            yield return new WaitForSeconds(time);
            // 清除受限UI
        }
    }

    // 死亡
    public void Die()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("I am dead");
            // TODO：弹出死亡界面
            Time.timeScale = 0;
        }
    }

    // 天敌发现，直接死亡
    public void GoDie()
    {
        currentHealth = 0;
    }

    // 摔落伤害
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            float fallDistance = maxHeight - transform.position.y;
            // 高于阈值
            if(fallDistance > heightThreshold)
            {
                float damage = (fallDistance - heightThreshold) * fallDamageRatio;
                TakeDamage(damage);
                // 落地后将最高高度重置
                maxHeight = transform.position.y;
            }
        }
    }
}
