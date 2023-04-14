using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // 控制移动速度
    public float speed = 5f;
    // 初始速度
    public float originalSpeed = 5f;
    // 转向速度
    public float turnSpeed = 180f;

    // 玩家
    public GameObject player;
    // 敌人
    public GameObject spider;
    // 远程攻击发射点
    public Transform launchPoint;
    // 远程攻击预制体
    [SerializeField] private GameObject bulletPerfab;
    // 检测范围
    [SerializeField] private float radius = 2f;
    // 远程攻击消耗
    [SerializeField] private float usage = 5f;
    // 远程技能伤害
    [SerializeField] private float damage = 10f;
    // 攻击间隔
    [SerializeField] private float duration = 5f;
    // 发射方向
    Vector3 launchDirection;
    // 发射速度大小
    public float bulletSpeed = 1f;
    // 是否攻击
    private bool canAttack = true;

    void Update()
    {
        Detect();
    }

    private void Detect()
    {
        RaycastHit hit;
        Vector3 relative = launchPoint.position - spider.transform.position;
        if (Physics.CapsuleCast(spider.transform.position, spider.transform.position + relative * radius, radius, relative, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                UseRangedAttack();
            }
        }
    }

    private void UseRangedAttack()
    {
        // 如果有体液值，进行attack2，否则需要移动到近战范围后进行attack1
        if (spider.GetComponent<Health>().currentBodyFluid >= usage)
        {
            launchDirection = player.transform.position - spider.transform.position;
            spider.transform.rotation = Quaternion.FromToRotation(spider.transform.forward, launchDirection);
            // 发射预制体
            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(RangedAttack());
            }
        }
    }

    IEnumerator RangedAttack()
    {
        spider.GetComponent<Health>().UseAttack(usage);
        // 碰撞后伤害在预制体代码中实现
        var animator = GetComponent<Animator>();
        animator.SetTrigger("attack2");
        GameObject bulletInstance = Instantiate(bulletPerfab, launchPoint.position, Quaternion.identity);
        Bullet bullet = bulletInstance.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.velocity = launchDirection.normalized * bulletSpeed;
            bullet.launchDirection = launchDirection;
            bullet.damagePerSecond = damage;
            Destroy(bulletInstance, 5f);
        }
        yield return new WaitForSeconds(duration);
        canAttack = true;
    }

    // 减速
    internal void Slow(float duration, float slowedSpeed)
    {
        StartCoroutine(SlowCoroutine(duration, slowedSpeed));
        // 将敌人速度设置为slowedSpeed

        IEnumerator SlowCoroutine(float duration, float slowedSpeed)
        {
            speed = slowedSpeed;

            // 等待指定的持续时间
            yield return new WaitForSeconds(duration);

            // 恢复敌人原始速度
            speed = originalSpeed;
        }
    }
}
