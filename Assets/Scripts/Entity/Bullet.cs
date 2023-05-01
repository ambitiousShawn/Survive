using Shawn.ProjectFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 投射物初速度
    public Vector3 velocity = Vector3.zero;
    // 每秒伤害
    public float damagePerSecond = 3f;
    // 减速时间长度
    public float slowDuration = 5f;
    // 减速比例
    public float slowRatio = 0.5f;
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

            // 视野受限
            BuffManager.Instance.AddBuff(0, () =>
            {
                target.gameObject.GetComponent<Player>().LimitedView(0.5f);
            });
            // 减速
            BuffManager.Instance.AddBuff(2, () =>
            {
                target.gameObject.GetComponent<PlayerController>().Slow(1.5f, slowRatio);
            });

            Destroy(gameObject);
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
        if(other.tag == "Player")
        {
            Destroy(gameObject);
        }
        target = null;
    }
}
