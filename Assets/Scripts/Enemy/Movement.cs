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
    // 玩家
    public GameObject player;
    // 敌人
    public GameObject spider;
    // 存储巡逻路线
    public Transform[] waypoints;
    // 当前巡逻路径下标
    private int waypointIndex = 0;
    // 是否检测到玩家
    private bool attack;
    // 检测范围
    [SerializeField] private float ridus = 10f;
    // 远程攻击消耗
    [SerializeField] private float usage = 5f;
    // 远程技能伤害
    [SerializeField] private float damage = 10f;
    // 攻击间隔
    [SerializeField] private float duration = 5f;

    // 计时器
    private float timer = 0f;

    void Update()
    {
        Vector3 relative = player.transform.position - spider.transform.position;
        if (relative.magnitude < 2 * ridus)
        {
            // 默认状态
            if (relative.magnitude > ridus)
            {
                Patrol();
            }
            else
            {
                attack = true;
                // 进行追击并攻击

                if (timer > duration)
                {
                    Attack();
                }
            }
        }

        if(attack)
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                Attack();
            }
        }
    }

    // 实现巡逻函数，简单AI
    void Patrol()
    {
        // 如果到达目标路径点，更新
        Vector3 way = waypoints[waypointIndex].position;
        if (spider.transform.position.x - way.x < 0.5f && spider.transform.position.z - way.z < 0.5f)
        {
            waypointIndex++;
            if(waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }

        Vector3 relative = new Vector3(way.x, 0, way.z);
        Vector3 pos = new Vector3(spider.transform.position.x,0, spider.transform.position.z);
        // 计算方向向量，并旋转敌人
        Vector3 targetDirection = relative - pos;
        transform.rotation = Quaternion.LookRotation(targetDirection);

        // 使用MoveTowards函数控制敌人项目表位置移动（时间平滑缓慢移动，通过Time.deltaTime控制）
        transform.position = Vector3.MoveTowards(pos, relative, speed * Time.deltaTime);
    }

    private void Attack()
    {
        // 如果有体液值，进行attack2，否则进行attack1
        var animator = GetComponent<Animator>();
        if (spider.GetComponent<Health>().currentBodyFluid >= usage)
        {
            animator.SetTrigger("attack2");
            spider.GetComponent<Health>().UseAttack(usage);
            player.GetComponent<Player>().TakeDamage(damage);
        }
        else
        {
            animator.SetTrigger("attack1");
        }

        timer = 0;
        attack = false;
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
