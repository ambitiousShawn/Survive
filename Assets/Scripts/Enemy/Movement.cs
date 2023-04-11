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
    // 存储巡逻路线
    public Transform[] waypoints;
    // 远程攻击发射点
    public Transform launchPoint;
    // 当前巡逻路径下标
    private int waypointIndex = 0;
    // 远程攻击预制体
    [SerializeField] private GameObject bulletPerfab;
    // 近战检测范围
    [SerializeField] private float ridus = 2f;
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
    // 巡航
    private bool isPatrol = true;
    // 靠近
    private bool isClose = false;

    void Update()
    {
        if (isPatrol)
        {
            Patrol();
        }

        if (isClose)
        {
            // 靠近，进入近战攻击范围后则停止
            Move();
        }

        // 相对距离
        Vector3 relative = player.transform.position - spider.transform.position;
        if (relative.magnitude < 5 * ridus)
        {
            // 玩家
            if (player.GetComponent<PlayerController>().isMoving)
            {
                isPatrol = false;
                // 远程攻击
                if (relative.magnitude > ridus)
                {
                    UseRangedAttack();
                }
                else // 进入近战范围进行攻击
                {
                    isClose = false;
                    if (canAttack)
                    {
                        canAttack = false;
                        StartCoroutine(Attack());
                    }
                }
            }
            else // 载具
            {
                isPatrol = true;
            }
        }
        else
        {
            isPatrol = true;
        }
    }

    // 实现巡逻函数，简单AI
    void Patrol()
    {
        // 如果到达目标路径点，更新
        Vector3 way = waypoints[waypointIndex].position;
        if (Mathf.Abs(spider.transform.position.x - way.x) < 0.05f && Mathf.Abs(spider.transform.position.z - way.z) < 0.05f)
        {
            Debug.Log(waypointIndex);
            waypointIndex++;
            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }

        Vector3 relative = new Vector3(way.x, 0, way.z);
        Vector3 pos = new Vector3(spider.transform.position.x, 0, spider.transform.position.z);
        // 计算方向向量，并旋转敌人
        Vector3 targetDirection = relative - pos;
        var rotate = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, turnSpeed * Time.deltaTime);

        // 使用MoveTowards函数控制敌人项目表位置移动（时间平滑缓慢移动，通过Time.deltaTime控制）
        transform.position = Vector3.MoveTowards(pos, relative, speed * Time.deltaTime);
    }

    IEnumerator Attack()
    {
        // 如果有体液值，进行attack2，否则进行attack1
        var animator = GetComponent<Animator>();
        animator.SetTrigger("attack1");
        player.GetComponent<Player>().TakeDamage(damage);

        yield return new WaitForSeconds(duration);
        canAttack = true;
    }

    private void UseRangedAttack()
    {
        // 如果有体液值，进行attack2，否则需要移动到近战范围后进行attack1
        if (spider.GetComponent<Health>().currentBodyFluid >= usage)
        {
            launchDirection = player.transform.position - spider.transform.position;
            // 发射预制体
            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(RangedAttack());
            }
        }
        else // 不能远程攻击靠近玩家
        {
            isClose = true;
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
            Destroy(bulletInstance, 5f);
        }
        yield return new WaitForSeconds(duration);
        canAttack = true;
    }

    // 靠近
    private void Move()
    {
        spider.transform.position = Vector3.MoveTowards(spider.transform.position, player.transform.position, speed * Time.deltaTime);
        var rot = Quaternion.LookRotation(player.transform.position);
        spider.transform.rotation = Quaternion.RotateTowards(spider.transform.rotation, rot, turnSpeed * Time.deltaTime);
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
