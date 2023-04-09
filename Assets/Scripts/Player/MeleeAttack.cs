using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeAttack : MonoBehaviour
{
    // 基础伤害值
    public float baseDamage = 10f;
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

    // 是否进行攻击
    private bool isAttacking = false;
    // 攻击范围检测器
    private SphereCollider attackCollider;

    public GameObject cameraPivot;
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
        if (player.GetComponent<PlayerController>().isMoving)
        {
            // 按下鼠标左键进行攻击
            if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1) && !isAttacking)
            {
                isAttacking = true;

                var animator = GetComponent<Animator>();
                if (animator != null)
                {
                    // 攻击动画
                    animator.SetTrigger("hit");

                    Invoke(nameof(DisableAttack), 1.0f);
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

    private void OnTriggerStay(Collider other)
    {
        if(!isAttacking)
        {
            return;
        }
        if(other.CompareTag("Enemy"))
        {
            // 伤害总大小
            var damageAmount = baseDamage;

            // 敌人承受伤害
            other.GetComponent<Health>().TakeDamage(damageAmount);

            // 敌人受击动画
            other.GetComponent<Animator>().SetTrigger("hit");

            // 测试抖动
            cameraPivot.GetComponent<CameraController>().ShakeCamera(5, 0.5f);

            Vector3 relative = other.transform.position - transform.position;
            Vector3 direction = new Vector3(relative.x, 0f, relative.z);

            // 敌人击退效果
            other.GetComponent<Enemy>().KnockBack(direction * knockBackForce, knockBackTime);

            // 敌人特效
            //Instantiate(hitEffectPerfab, other.ClosestPoint(transform.position), Quaternion.identity);
            //Destroy();
        }
    }

    private void DisableAttack()
    {
        isAttacking = false;
        attackCollider.enabled = false;
    }
}
