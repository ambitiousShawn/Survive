using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shawn.ProjectFramework;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    private UGUI_MainUIPanel panel;

    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.1
    // @brief       角色属性
    //-----------------------------------------------------


    
    [Header("角色生命值信息")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] public static float currentHealth;

    [Header("角色体液值信息")]
    [SerializeField] private float maxBodyFluid = 100f;
    [SerializeField] public static float currentBodyFluid;

    // 每秒回复值
    [SerializeField] private float pickUpPerSecond = 0.05f;

    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.1
    // @brief       摔落伤害数据
    //-----------------------------------------------------

    // 高度阈值，在此之上产生摔落伤害
    [Header("摔落伤害相关")]
    [SerializeField] private float heightThreshold = 5f;
    // 摔落伤害系数
    [SerializeField] private float fallDamageRatio = 2f;
    // 记录最高高度
    [SerializeField] private float maxHeight;

    void Start()
    {
        currentHealth = maxHealth;
        currentBodyFluid = maxBodyFluid;
    }

    private void Update()
    {
        // 在这里调用主UI更新生命值、体液值和Buff
        if (panel == null)
            panel = PanelManager.Instance.GetPanelByName("UGUI_MainUIPanel") as UGUI_MainUIPanel;

        // 角色控制时检测摔落伤害和死亡
        if (GetComponent<PlayerController>().isMoving)
        {
            //TODO:衰落实现需要改进，如果存在玩家先到高处然后到一个较低的悬崖，衰落会计算最高的高度。
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
        TakeDamage(damage);
    }

    // 对角色造成伤害
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Die();
        panel.UpdateHealthBar(currentHealth);
    }
    // 体液值降低
    public void UseRangedAttack(float usage)
    {
        currentBodyFluid -= usage;
        if (currentBodyFluid <= 0)
            currentBodyFluid = 0;
        panel.UpdateSkillBar(currentBodyFluid);
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

    /*// debuff持续伤害，忍耐值
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
    }*/

    /*// debuff视野受限，参数为时长
    public void LimitedView(float time)
    {
        StartCoroutine(AddLimited());
        IEnumerator AddLimited()
        {
            // TODO：添加视野受限效果
            yield return new WaitForSeconds(time);
            // 清除受限UI
        }
    }*/

    // 死亡
    public void Die()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            currentBodyFluid = 0;
            Debug.Log("I am dead");
            // TODO:弹出死亡界面
            //Time.timeScale = 0;
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
