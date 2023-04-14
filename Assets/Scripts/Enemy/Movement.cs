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
    // Զ�̹��������
    public Transform launchPoint;
    // Զ�̹���Ԥ����
    [SerializeField] private GameObject bulletPerfab;
    // ��ⷶΧ
    [SerializeField] private float radius = 2f;
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
        // �������Һֵ������attack2��������Ҫ�ƶ�����ս��Χ�����attack1
        if (spider.GetComponent<Health>().currentBodyFluid >= usage)
        {
            launchDirection = player.transform.position - spider.transform.position;
            spider.transform.rotation = Quaternion.FromToRotation(spider.transform.forward, launchDirection);
            // ����Ԥ����
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
        // ��ײ���˺���Ԥ���������ʵ��
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
