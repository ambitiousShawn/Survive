using Shawn.ProjectFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Ͷ������ٶ�
    public Vector3 velocity = Vector3.zero;
    // ÿ���˺�
    public float damagePerSecond = 3f;
    // ����ʱ�䳤��
    public float slowDuration = 5f;
    // ���ٱ���
    public float slowRatio = 0.5f;
    // ��������
    public Vector3 launchDirection;

    private Rigidbody rb;
    private Collider target;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = velocity;
    }

    void FixedUpdate()
    {
        // ͬ����ɼ���Ч��
        if (target != null)
        {
            transform.LookAt(target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 2f);

            // ��Ұ����
            BuffManager.Instance.AddBuff(0, () =>
            {
                target.gameObject.GetComponent<Player>().LimitedView(0.5f);
            });
            // ����
            BuffManager.Instance.AddBuff(2, () =>
            {
                target.gameObject.GetComponent<PlayerController>().Slow(1.5f, slowRatio);
            });

            Destroy(gameObject);
        }
    }

    // �������
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
        }
        target = null;
    }
}
