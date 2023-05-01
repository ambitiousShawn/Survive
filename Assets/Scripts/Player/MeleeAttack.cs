using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeAttack : MonoBehaviour
{
    // 伤害比例
    public float damageRatio = 0.3f;
    // 攻击范围
    public float attackRange = 1f;
    // 击退力度
    public float knockBackForce = 10f;
    // 击退时间
    public float knockBackTime = 1f;
    // 击中特效预制件
    public GameObject hitEffectPerfab;
    // 特效持续时间
    public float hitEffectDuration = 1f;
    // 攻击间隔
    [SerializeField] private float attackInterval;

    // 是否进行攻击
    private bool isAttacking = false;
    // 攻击范围检测器
    private SphereCollider attackCollider;
    public GameObject player;

    private void Start()
    {
        attackCollider = gameObject.AddComponent<SphereCollider>();
        attackCollider.isTrigger = true;
        attackCollider.radius = attackRange;
        // 初始不启用
        attackCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 如果是玩家控制才可以近战攻击
        if (player.GetComponent<PlayerController>().isMoving)
        {
            // 按下鼠标左键进行攻击且不在攻击cd中
            // 区分近战远程需要判断鼠标右键状态
            if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1) && !isAttacking)
            {
                isAttacking = true;

                var animator = GetComponent<Animator>();
                if (animator != null)
                {
                    // 攻击动画
                    animator.SetTrigger("hit");

                    // 攻击间隔
                    StartCoroutine(DisableAttack());
                }

                //启用碰撞器
                attackCollider.enabled = true;
            }
            else
            {
                isAttacking = false;
            }
        }
    }

    IEnumerator DisableAttack()
    {
        isAttacking = false;
        attackCollider.enabled = false;
        yield return new WaitForSeconds(attackInterval);
    }

    // 近战范围敌人检测
    private void OnTriggerStay(Collider other)
    {
        if(!isAttacking)
        {
            return;
        }
        // 目前只可攻击近战敌人
        if(other.CompareTag("MeleeEnemy"))
        {
            // 伤害总大小，最大生命值的比例伤害
            var damageAmount = damageRatio * other.GetComponent<Health>().maxHealth;

            // 敌人承受伤害
            other.GetComponent<Health>().TakeDamage(damageAmount);

            // 敌人受击动画
            other.GetComponent<Animator>().SetTrigger("hit");

            Vector3 relative = other.transform.position - transform.position;
            Vector3 direction = new Vector3(relative.x, 0f, relative.z);

            // 敌人击退效果
            other.GetComponent<Enemy>().KnockBack(direction * knockBackForce, knockBackTime);

            // 敌人特效
            //Instantiate(hitEffectPerfab, other.ClosestPoint(transform.position), Quaternion.identity);
            //Destroy();
        }
    }
}
