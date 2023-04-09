using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    // Ͷ������ٶ�
    public Vector3 velocity;
    // ÿ���˺�
    public float damagePerSecond;
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
        if (target != null )
        {
            transform.LookAt(target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 2f);
            target.gameObject.GetComponent<Movement>().Slow(slowDuration, .2f);
            target.gameObject.GetComponent<Health>().TakeDamage(damagePerSecond * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy")
        {
            target = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        target = null;
    }
}