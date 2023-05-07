using Shawn.ProjectFramework;
using System.Collections;
using UnityEngine;

public class MelleEnemy : FSM
{
    // �����ƶ��ٶ�
    public float speed = 5f;
    // ��ʼ�ٶ�
    public float originalSpeed = 5f;
    // ת���ٶ�
    public float turnSpeed = 180f;

    // ���
    public GameObject player;
    // ����
    public GameObject enemy;
    // �洢Ѳ��·��
    public Transform[] waypoints;

    // ��ǰѲ��·���±�
    private int waypointIndex = 0;
    // ��ս��ⷶΧ
    [SerializeField] private float ridus = 2f;
    // �˺�
    [SerializeField] private float damage = 10f;
    // �������
    [SerializeField] private float duration = 5f;

    // �Ƿ񹥻�
    private bool canAttack = true;
    // Ѳ��
    private bool isPatrol = true;
    // ����
    private bool isClose = false;

    void Update()
    {
        if (isPatrol)
        {
            Patrol();
        }

        if (isClose)
        {
            // �����������ս������Χ����ֹͣ
            Move();
        }

        // ��Ծ���
        Vector3 relative = player.transform.position - enemy.transform.position;
        if (relative.magnitude < 5 * ridus)
        {
            // ���
            if (player.GetComponent<PlayerController>().isMoving && !player.GetComponent<PlayerController>().isHide)
            {
                isPatrol = false;
                // Զ�̹���
                if (relative.magnitude > ridus)
                {
                    isClose = true;
                }
                else // �����ս��Χ���й���
                {
                    isClose = false;
                    if (canAttack)
                    {
                        canAttack = false;
                        StartCoroutine(Attack());
                    }
                }
            }
            else // �ؾ�
            {
                isPatrol = true;
            }
        }
        else
        {
            isPatrol = true;
        }
    }

    // ʵ��Ѳ�ߺ�������AI
    void Patrol()
    {
        // �������Ŀ��·���㣬����
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
        // ���㷽������������ת����
        Vector3 targetDirection = relative - pos;
        var rotate = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, turnSpeed * Time.deltaTime);

        // ʹ��MoveTowards�������Ƶ�����Ŀ��λ���ƶ���ʱ��ƽ�������ƶ���ͨ��Time.deltaTime���ƣ�
        transform.position = Vector3.MoveTowards(pos, relative, speed * Time.deltaTime);
    }

    IEnumerator Attack()
    {
        // �������Һֵ������attack2���������attack1
        var animator = GetComponent<Animator>();

        // ����
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

    // ����
    private void Move()
    {
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, speed * Time.deltaTime);
        var rot = Quaternion.LookRotation(player.transform.position);
        enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, rot, turnSpeed * Time.deltaTime);
    }

    // ����
    internal void Slow(float duration, float slowedSpeed)
    {
        StartCoroutine(SlowCoroutine(duration, slowedSpeed));
        // �������ٶ�����ΪslowedSpeed

        IEnumerator SlowCoroutine(float duration, float slowedSpeed)
        {
            speed = slowedSpeed;

            // �ȴ�ָ���ĳ���ʱ��
            yield return new WaitForSeconds(duration);

            // �ָ�����ԭʼ�ٶ�
            speed = originalSpeed;
        }
    }


    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.0
    // @brief       ��״̬������AI���ҡ���
    //-----------------------------------------------------
    //// �����ƶ��ٶ�
    //public float speed = 5f;
    //// ��ʼ�ٶ�
    //public float originalSpeed = 5f;
    //// ת���ٶ�
    //public float turnSpeed = 180f;

    //// ���
    //public GameObject player;
    //// ����
    //public GameObject enemy;
    //// �洢Ѳ��·��
    //public Transform[] waypoints;

    //// ��ǰѲ��·���±�
    //private int waypointIndex = 0;
    //// ��ս��ⷶΧ
    //[SerializeField] private float ridus = 2f;
    //// �˺�
    //[SerializeField] private float damage = 10f;
    //// �������
    //[SerializeField] private float duration = 5f;

    //[SerializeField] private float slowDuration = 2f;

    //[SerializeField] private float ratio = 0.3f;

    //[SerializeField] private float limitedTime = 0.5f;
    //[SerializeField] private float limitedRange = 3f;

    //// �Ƿ񹥻�
    //private bool canAttack = true;
    //// Ѳ��
    //private bool isPatrol = true;
    //// ����
    //private bool isClose = false;

    //void Update()
    //{
    //    if (isPatrol)
    //    {
    //        Patrol();
    //    }

    //    if (isClose)
    //    {
    //        // �����������ս������Χ����ֹͣ
    //        Move();
    //    }

    //    // ��Ծ���
    //    Vector3 relative = player.transform.position - enemy.transform.position;
    //    if (relative.magnitude < 5 * ridus)
    //    {
    //        // ���
    //        if (player.GetComponent<PlayerController>().isMoving && !player.GetComponent<PlayerController>().isHide)
    //        {
    //            isPatrol = false;
    //            // Զ�̹���
    //            if (relative.magnitude > ridus)
    //            {
    //                isClose = true;
    //            }
    //            else // �����ս��Χ���й���
    //            {
    //                isClose = false;
    //                if (canAttack)
    //                {
    //                    canAttack = false;
    //                    StartCoroutine(Attack());
    //                }
    //            }
    //        }
    //        else // �ؾ�
    //        {
    //            isPatrol = true;
    //        }
    //    }
    //    else
    //    {
    //        isPatrol = true;
    //    }
    //}

    //// ʵ��Ѳ�ߺ�������AI
    //void Patrol()
    //{
    //    // �������Ŀ��·���㣬����
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
    //    // ���㷽������������ת����
    //    Vector3 targetDirection = relative - pos;
    //    var rotate = Quaternion.LookRotation(targetDirection);
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, turnSpeed * Time.deltaTime);

    //    // ʹ��MoveTowards�������Ƶ�����Ŀ��λ���ƶ���ʱ��ƽ�������ƶ���ͨ��Time.deltaTime���ƣ�
    //    transform.position = Vector3.MoveTowards(pos, relative, speed * Time.deltaTime);
    //}

    //IEnumerator Attack()
    //{
    //    // �������Һֵ������attack2���������attack1
    //    var animator = GetComponent<Animator>();

    //    // ����
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

    //// ����
    //private void Move()
    //{
    //    enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, speed * Time.deltaTime);
    //    var rot = Quaternion.LookRotation(player.transform.position);
    //    enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, rot, turnSpeed * Time.deltaTime);
    //}

    //// ����
    //internal void Slow(float duration, float slowedSpeed)
    //{
    //    StartCoroutine(SlowCoroutine(duration, slowedSpeed));
    //    // �������ٶ�����ΪslowedSpeed

    //    IEnumerator SlowCoroutine(float duration, float slowedSpeed)
    //    {
    //        speed = slowedSpeed;

    //        // �ȴ�ָ���ĳ���ʱ��
    //        yield return new WaitForSeconds(duration);

    //        // �ָ�����ԭʼ�ٶ�
    //        speed = originalSpeed;
    //    }
    //}
}
