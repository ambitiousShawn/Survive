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
    // ���
    public GameObject player;
    // ����
    public GameObject spider;
    // �洢Ѳ��·��
    public Transform[] waypoints;
    // ��ǰѲ��·���±�
    private int waypointIndex = 0;
    // �Ƿ��⵽���
    private bool attack;
    // ��ⷶΧ
    [SerializeField] private float ridus = 10f;
    // Զ�̹�������
    [SerializeField] private float usage = 5f;
    // Զ�̼����˺�
    [SerializeField] private float damage = 10f;
    // �������
    [SerializeField] private float duration = 5f;

    // ��ʱ��
    private float timer = 0f;

    void Update()
    {
        Vector3 relative = player.transform.position - spider.transform.position;
        if (relative.magnitude < 2 * ridus)
        {
            // Ĭ��״̬
            if (relative.magnitude > ridus)
            {
                Patrol();
            }
            else
            {
                attack = true;
                // ����׷��������

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

    // ʵ��Ѳ�ߺ�������AI
    void Patrol()
    {
        // �������Ŀ��·���㣬����
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
        // ���㷽������������ת����
        Vector3 targetDirection = relative - pos;
        transform.rotation = Quaternion.LookRotation(targetDirection);

        // ʹ��MoveTowards�������Ƶ�����Ŀ��λ���ƶ���ʱ��ƽ�������ƶ���ͨ��Time.deltaTime���ƣ�
        transform.position = Vector3.MoveTowards(pos, relative, speed * Time.deltaTime);
    }

    private void Attack()
    {
        // �������Һֵ������attack2���������attack1
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
