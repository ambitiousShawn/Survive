using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // ���
    public GameObject player;
    // ����
    public GameObject spider;
    // Զ�̹��������
    public Transform launchPoint;
    // Զ�̹���Ԥ����
    [SerializeField] private GameObject bulletPerfab;
    // ��ⷶΧ��Բ׶��Χ�������
    [SerializeField] private float radius = 2f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float angle = 15;
    // Զ�̹�������
    [SerializeField] private float usage = 5f;
    // Զ�̼����˺�
    [SerializeField] private float damage = 10f;
    // �������
    [SerializeField] private float duration = 5f;
    // ���䷽��
    Vector3 launchDirection;
    // �Ƿ񹥻�
    private bool canAttack = true;


    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.1
    // @brief       Ԥ��켣
    //-----------------------------------------------------
    public float maxAttackRange = 5f;
    public float attackDuration = 2f;
    // ˮƽ�ٶȣ�����������
    public float projectileSpeed = 1f;

    // �����ߵ�
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
        // ˮƽ���
        relative = new Vector3(enemyToPlayer.x, 0, enemyToPlayer.z);
        float horizontalDistance = relative.magnitude;

        if (enemyToPlayer.magnitude < maxAttackRange)
        {
            // ����ˮƽ�����ʱ��
            flightDuration = horizontalDistance / projectileSpeed;
            float delta = Mathf.Pow(flightDuration, 2) * (peakHeight + Mathf.Abs(enemyToPlayer.y)) / peakHeight;
            // ������Ҫ�Ǳ������
            if (delta >= 0)
            {
                // ����һ��ʱ��
                float timeToPeak = (Mathf.Sqrt(delta) - flightDuration) * peakHeight / Mathf.Abs(enemyToPlayer.y);

                Debug.DrawLine(enemyPos, enemyPos + relative.normalized * timeToPeak * horizontalSpeed);
                // �õ���ֱ������ٶ�
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
        // ����Ԥ����
        if (canAttack)
        {
            canAttack = false;
            StartCoroutine(RangedAttack());
        }
    }
    IEnumerator RangedAttack()
    {
        spider.GetComponent<Enemy>().UseAttack(usage);
        // ��ײ���˺���Ԥ���������ʵ��
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
