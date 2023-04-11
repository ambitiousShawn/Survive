using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Ͷ������ٶ�
    public Vector3 velocity;
    // ÿ���˺�
    public float damagePerSecond = 3f;
    // ����ʱ�䳤��
    public float slowDuration = 5f;
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
            // ��Buffʵ��
            target.gameObject.GetComponent<PlayerController>().Slow(slowDuration, .2f);
            target.gameObject.GetComponent<Player>().TakeDamage(damagePerSecond * Time.fixedDeltaTime);
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
        target = null;
    }
}
