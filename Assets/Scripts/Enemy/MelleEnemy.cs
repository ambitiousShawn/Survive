using Shawn.ProjectFramework;
using System.Collections;
using UnityEngine;

public class MelleEnemy : FSM
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
    public GameObject enemy;
    // 存储巡逻路线
    public Transform[] waypoints;

    // 当前巡逻路径下标
    private int waypointIndex = 0;
    // 近战检测范围
    [SerializeField] private float ridus = 2f;
    // 伤害
    [SerializeField] private float damage = 10f;
    // 攻击间隔
    [SerializeField] private float duration = 5f;

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
        Vector3 relative = player.transform.position - enemy.transform.position;
        if (relative.magnitude < 5 * ridus)
        {
            // 玩家
            if (player.GetComponent<PlayerController>().isMoving && !player.GetComponent<PlayerController>().isHide)
            {
                isPatrol = false;
                // 远程攻击
                if (relative.magnitude > ridus)
                {
                    isClose = true;
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
        if (Mathf.Abs(enemy.transform.position.x - way.x) < 0.05f && Mathf.Abs(enemy.transform.position.z - way.z) < 0.05f)
        {
            Debug.Log(waypointIndex);
            waypointIndex++;
            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }

        Vector3 relative = way;
        Vector3 pos = enemy.transform.position;
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

        // 动画
        //animator.SetTrigger("attack1");
        player.GetComponent<Player>().TakeDamage(damage);

        BuffManager.Instance.AddBuff(0, () =>
        {
            //splayer.GetComponent<Player>().LimitedView(limitedTime);
        });

        BuffManager.Instance.AddBuff(2, () =>
        {
            //player.GetComponent<PlayerController>().Slow(slowDuration, ratio);
        });

        yield return new WaitForSeconds(duration);
        canAttack = true;
    }

    // 靠近
    private void Move()
    {
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, speed * Time.deltaTime);
        var rot = Quaternion.LookRotation(player.transform.position);
        enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, rot, turnSpeed * Time.deltaTime);
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


    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.0
    // @brief       非状态机敌人AI，乱……
    //-----------------------------------------------------
    //// 控制移动速度
    //public float speed = 5f;
    //// 初始速度
    //public float originalSpeed = 5f;
    //// 转向速度
    //public float turnSpeed = 180f;

    //// 玩家
    //public GameObject player;
    //// 敌人
    //public GameObject enemy;
    //// 存储巡逻路线
    //public Transform[] waypoints;

    //// 当前巡逻路径下标
    //private int waypointIndex = 0;
    //// 近战检测范围
    //[SerializeField] private float ridus = 2f;
    //// 伤害
    //[SerializeField] private float damage = 10f;
    //// 攻击间隔
    //[SerializeField] private float duration = 5f;

    //[SerializeField] private float slowDuration = 2f;

    //[SerializeField] private float ratio = 0.3f;

    //[SerializeField] private float limitedTime = 0.5f;
    //[SerializeField] private float limitedRange = 3f;

    //// 是否攻击
    //private bool canAttack = true;
    //// 巡航
    //private bool isPatrol = true;
    //// 靠近
    //private bool isClose = false;

    //void Update()
    //{
    //    if (isPatrol)
    //    {
    //        Patrol();
    //    }

    //    if (isClose)
    //    {
    //        // 靠近，进入近战攻击范围后则停止
    //        Move();
    //    }

    //    // 相对距离
    //    Vector3 relative = player.transform.position - enemy.transform.position;
    //    if (relative.magnitude < 5 * ridus)
    //    {
    //        // 玩家
    //        if (player.GetComponent<PlayerController>().isMoving && !player.GetComponent<PlayerController>().isHide)
    //        {
    //            isPatrol = false;
    //            // 远程攻击
    //            if (relative.magnitude > ridus)
    //            {
    //                isClose = true;
    //            }
    //            else // 进入近战范围进行攻击
    //            {
    //                isClose = false;
    //                if (canAttack)
    //                {
    //                    canAttack = false;
    //                    StartCoroutine(Attack());
    //                }
    //            }
    //        }
    //        else // 载具
    //        {
    //            isPatrol = true;
    //        }
    //    }
    //    else
    //    {
    //        isPatrol = true;
    //    }
    //}

    //// 实现巡逻函数，简单AI
    //void Patrol()
    //{
    //    // 如果到达目标路径点，更新
    //    Vector3 way = waypoints[waypointIndex].position;
    //    if (Mathf.Abs(enemy.transform.position.x - way.x) < 0.05f && Mathf.Abs(enemy.transform.position.z - way.z) < 0.05f)
    //    {
    //        Debug.Log(waypointIndex);
    //        waypointIndex++;
    //        if (waypointIndex >= waypoints.Length)
    //        {
    //            waypointIndex = 0;
    //        }
    //    }

    //    Vector3 relative = way;
    //    Vector3 pos = enemy.transform.position;
    //    // 计算方向向量，并旋转敌人
    //    Vector3 targetDirection = relative - pos;
    //    var rotate = Quaternion.LookRotation(targetDirection);
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, turnSpeed * Time.deltaTime);

    //    // 使用MoveTowards函数控制敌人项目表位置移动（时间平滑缓慢移动，通过Time.deltaTime控制）
    //    transform.position = Vector3.MoveTowards(pos, relative, speed * Time.deltaTime);
    //}

    //IEnumerator Attack()
    //{
    //    // 如果有体液值，进行attack2，否则进行attack1
    //    var animator = GetComponent<Animator>();

    //    // 动画
    //    //animator.SetTrigger("attack1");
    //    player.GetComponent<Player>().TakeDamage(damage);

    //    BuffManager.Instance.AddBuff(0, () =>
    //    {
    //        player.GetComponent<Player>().LimitedView(limitedTime, limitedRange);
    //    });

    //    BuffManager.Instance.AddBuff(2, () =>
    //    {
    //        player.GetComponent<PlayerController>().Slow(slowDuration, ratio);
    //    });

    //    yield return new WaitForSeconds(duration);
    //    canAttack = true;
    //}

    //// 靠近
    //private void Move()
    //{
    //    enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, speed * Time.deltaTime);
    //    var rot = Quaternion.LookRotation(player.transform.position);
    //    enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, rot, turnSpeed * Time.deltaTime);
    //}

    //// 减速
    //internal void Slow(float duration, float slowedSpeed)
    //{
    //    StartCoroutine(SlowCoroutine(duration, slowedSpeed));
    //    // 将敌人速度设置为slowedSpeed

    //    IEnumerator SlowCoroutine(float duration, float slowedSpeed)
    //    {
    //        speed = slowedSpeed;

    //        // 等待指定的持续时间
    //        yield return new WaitForSeconds(duration);

    //        // 恢复敌人原始速度
    //        speed = originalSpeed;
    //    }
    //}
}
