using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
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
    public GameObject spider;
    // �洢Ѳ��·��
    public Transform[] waypoints;
    // Զ�̹��������
    public Transform launchPoint;
    // ��ǰѲ��·���±�
    private int waypointIndex = 0;
    // Զ�̹���Ԥ����
    [SerializeField] private GameObject bulletPerfab;
    // ��ս��ⷶΧ
    [SerializeField] private float ridus = 2f;
    // Զ�̹�������
    [SerializeField] private float usage = 5f;
    // Զ�̼����˺�
    [SerializeField] private float damage = 10f;
    // �������
    [SerializeField] private float duration = 5f;
    // ���䷽��
    Vector3 launchDirection;
    // �����ٶȴ�С
    public float bulletSpeed = 1f;
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
        Vector3 relative = player.transform.position - spider.transform.position;
        if (relative.magnitude < 5 * ridus)
        {
            // ���
            if (player.GetComponent<PlayerController>().isMoving)
            {
                isPatrol = false;
                // Զ�̹���
                if (relative.magnitude > ridus)
                {
                    UseRangedAttack();
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
        animator.SetTrigger("attack1");
        player.GetComponent<Player>().TakeDamage(damage);

        yield return new WaitForSeconds(duration);
        canAttack = true;
    }

    private void UseRangedAttack()
    {
        // �������Һֵ������attack2��������Ҫ�ƶ�����ս��Χ�����attack1
        if (spider.GetComponent<Health>().currentBodyFluid >= usage)
        {
            launchDirection = player.transform.position - spider.transform.position;
            // ����Ԥ����
            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(RangedAttack());
            }
        }
        else // ����Զ�̹����������
        {
            isClose = true;
        }
    }

    IEnumerator RangedAttack()
    {
        spider.GetComponent<Health>().UseAttack(usage);
        // ��ײ���˺���Ԥ���������ʵ��
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

    // ����
    private void Move()
    {
        spider.transform.position = Vector3.MoveTowards(spider.transform.position, player.transform.position, speed * Time.deltaTime);
        var rot = Quaternion.LookRotation(player.transform.position);
        spider.transform.rotation = Quaternion.RotateTowards(spider.transform.rotation, rot, turnSpeed * Time.deltaTime);
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
}
