using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 投射物初速度
    public Vector3 velocity;
    // 每秒伤害
    public float damagePerSecond = 3f;
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
        // 同样造成减速效果
        if (target != null)
        {
            transform.LookAt(target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 2f);
            // 换Buff实现
            target.gameObject.GetComponent<PlayerController>().Slow(slowDuration, .2f);
            target.gameObject.GetComponent<Player>().TakeDamage(damagePerSecond * Time.fixedDeltaTime);
        }
    }

    // 攻击玩家
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
