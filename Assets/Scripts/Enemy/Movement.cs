using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // 玩家
    public GameObject player;
    // 敌人
    public GameObject spider;
    // 远程攻击发射点
    public Transform launchPoint;
    // 远程攻击预制体
    [SerializeField] private GameObject bulletPerfab;
    // 检测范围，圆锥范围相关数据
    [SerializeField] private float radius = 2f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float angle = 15;
    // 远程攻击消耗
    [SerializeField] private float usage = 5f;
    // 远程技能伤害
    [SerializeField] private float damage = 10f;
    // 攻击间隔
    [SerializeField] private float duration = 5f;
    // 发射方向
    Vector3 launchDirection;
    // 是否攻击
    private bool canAttack = true;


    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.1
    // @brief       预测轨迹
    //-----------------------------------------------------
    public float maxAttackRange = 5f;
    public float attackDuration = 2f;
    // 水平速度，决定多快击中
    public float projectileSpeed = 1f;

    // 相对最高点
    [SerializeField] private float peakHeight = 1f;

    private float flightDuration;
    private float verticalSpeed;
    private float horizontalSpeed;
    private Vector3 relative;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 origin = transform.position;
            Vector3 direction = other.transform.position - origin;
            float verticalDistance = Vector3.Dot(direction.normalized, transform.up) * direction.magnitude;
            

            if (verticalDistance < height)
            {
                float horizontalDistance = Vector3.Dot(direction.normalized, transform.right) * direction.magnitude;

                float hDistAlongCone = (horizontalDistance / Mathf.Cos(angle / 2));
                if (Mathf.Abs(hDistAlongCone) < radius)
                {
                    UseRangedAttack();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 forward = transform.forward;

        Vector3 left = Quaternion.Euler(0f, -angle / 2f, 0f) * forward;
        Vector3 right = Quaternion.Euler(0f, angle / 2f, 0f) * forward;

        Vector3 bottomLeft = transform.position + left * radius;
        Vector3 bottomRight = transform.position + right * radius;

        Gizmos.DrawLine(transform.position, bottomLeft);
        Gizmos.DrawLine(transform.position, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);

        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawRay(transform.position, transform.up * height);
    }

    private void UseRangedAttack()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = launchPoint.position;

        Vector3 enemyToPlayer = playerPos - enemyPos;
        // 水平相对
        relative = new Vector3(enemyToPlayer.x, 0, enemyToPlayer.z);
        float horizontalDistance = relative.magnitude;

        if (enemyToPlayer.magnitude < maxAttackRange)
        {
            // 根据水平求解总时间
            flightDuration = horizontalDistance / projectileSpeed;
            float delta = Mathf.Pow(flightDuration, 2) * (peakHeight + Mathf.Abs(enemyToPlayer.y)) / peakHeight;
            // 这里主要是避免出错
            if (delta >= 0)
            {
                // 求解第一段时间
                float timeToPeak = (Mathf.Sqrt(delta) - flightDuration) * peakHeight / Mathf.Abs(enemyToPlayer.y);

                Debug.DrawLine(enemyPos, enemyPos + relative.normalized * timeToPeak * horizontalSpeed);
                // 得到竖直方向初速度
                verticalSpeed = timeToPeak * -Physics.gravity.y;
                horizontalSpeed = projectileSpeed;

                float peakDistance = horizontalSpeed * timeToPeak;

                Vector3 peakPoint = enemyPos + relative.normalized * peakDistance + Vector3.up * peakHeight;

                Debug.DrawLine(enemyPos, peakPoint, Color.green);
                Debug.DrawLine(peakPoint, playerPos, Color.yellow);

                for (float t = 0f; t < attackDuration; t += 0.1f)
                {
                    Vector3 p = CalculateProjectilePosition(enemyPos, t);
                    Debug.DrawLine(p, p + Vector3.up, Color.red);
                }

                launchDirection = relative.normalized * horizontalSpeed + Vector3.up * verticalSpeed;
                Debug.DrawLine(enemyPos, enemyPos + launchDirection, Color.green);
            }
        }
        // 发射预制体
        if (canAttack)
        {
            canAttack = false;
            StartCoroutine(RangedAttack());
        }
    }
    IEnumerator RangedAttack()
    {
        spider.GetComponent<Enemy>().UseAttack(usage);
        // 碰撞后伤害在预制体代码中实现
        var animator = GetComponent<Animator>();
        animator.SetTrigger("attack2");
        GameObject bulletInstance = Instantiate(bulletPerfab, launchPoint.position, Quaternion.identity);
        Bullet bullet = bulletInstance.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.velocity = launchDirection;
            bullet.damagePerSecond = damage;
            if (bulletInstance != null)
            {
                Destroy(bulletInstance, 5f);
            }
        }
        yield return new WaitForSeconds(duration);
        canAttack = true;
    }

    private Vector3 CalculateProjectilePosition(Vector3 origin, float timeInFlight)
    {
        float y = verticalSpeed * timeInFlight + (0.5f * Physics.gravity.y * Mathf.Pow(timeInFlight, 2f));

        Vector3 result = origin + relative.normalized * horizontalSpeed + Vector3.up * y;

        return result;
    }
}
