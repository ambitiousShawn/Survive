using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shawn.ProjectFramework;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    public GameObject CameraPivot;

    // 生命值
    public const float maxHealth = 100f;
    public float currentHealth;
    // 体液值
    public const float maxBodyFluid = 100f;
    public float currentBodyFluid;
    // 每秒回复值
    public float pickUpPerSecond = 0.05f;

    public float damagePerSecond = 1f;
    UGUI_MainUIPanel panel;

    // 高度，在此之上产生摔落伤害
    [SerializeField] private float height = 5f;
    [SerializeField] private float fallDamageRatio = 2f;

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

            // 同样更新Buff和物品栏
        }

        // 角色控制时检测摔落伤害和死亡
        if (gameObject.GetComponent<PlayerController>().isMoving)
        {
            FallDamage();
            Die();
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentBodyFluid = maxBodyFluid;
        rb = GetComponent<Rigidbody>();
    }

    // 生命值降低
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    // 摔落伤害
    private void FallDamage()
    {
        // 根据速度来确定伤害
        float criticalVelocity = Mathf.Sqrt(-2 * height * Physics.gravity.y);
        bool touchGround = gameObject.GetComponent<PlayerController>().isGrounded;
        float fallDamage = (Mathf.Pow(rb.velocity.y, 2) / -2 / Physics.gravity.y - height) * fallDamageRatio;
        // 落地
        if (rb.velocity.y < -criticalVelocity && touchGround)
        {
            bool trigger = true;
            if (trigger)
            {
                // 协程
                StartCoroutine(Fall());
                IEnumerator Fall()
                {
                    TakeDamage(fallDamage);
                    Debug.Log("FallDamage!");

                    yield return new WaitForSeconds(1);
                }
                trigger = false;
            }
        }
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

    // debuff眩晕，参数为持续时间和晃动幅度
    private void Vertigo(float duration, float magnitude)
    {
        CameraPivot.GetComponent<CameraController>().ShakeCamera(duration, magnitude);
        // 增添雾化效果，在UI上实现
    }

    // debuff视野受限，参数为时长和范围
    private void LimitedView(float time, float range)
    {
        // 记录初始视野大小
        float size = Camera.main.orthographicSize;
        StartCoroutine(Timer());
        IEnumerator Timer()
        {
            Camera.main.orthographicSize = range;
            yield return new WaitForSeconds(time);
        }
        // 恢复原始大小
        Camera.main.orthographicSize = size;
    }

    // debuff持续伤害，忍耐值
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

    // 死亡
    public void Die()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("I am dead");
            Time.timeScale = 0;
        }
    }

    public void GoDie()
    {
        currentHealth = 0;
    }
}
