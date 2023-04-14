using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int ID;

    // 投射物初速度
    public Vector3 velocity;
    // 每秒伤害
    public float damagePerSecond;
    // 减速时间长度
    public float slowDuration = 5f;
    // 正向喷射
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
        if(other.tag == "Enemy")
        {
            target = other;
        }
        else if(ID == 1 && other.tag == "Organ1")
        {
            Destroy(other.gameObject);
        }
        else if(ID == 2 && other.tag == "Organ2")
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        target = null;
    }
}