using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attack;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;

public class Projectile : MonoBehaviour
{
    [SerializeField] public SkillState ID;

    // 投射物初速度
    public Vector3 velocity;
    // 每秒伤害
    public float damageRatio;
    // debuff持续时长
    public float duration = 5f;

    private Rigidbody rb;
    private Collider target;

    // 存储相关数据
    private List<AttackData> attackData;
    private AttackData attack;

    private const string Attack_Path = "SO/Attack_SO";

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = velocity;

        attackData = Resources.Load<AttackData_SO>(Attack_Path).root;
        attack = attackData[(int)ID];
        damageRatio = attack.damageRatio;
        duration = attack.duration;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            target = other;
            // 朝向和吸附
            transform.LookAt(target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 2f);

            // TODO：委托debuff
            //attack.doEffect();
            target.gameObject.GetComponent<Enemy>().TakeDamage(damageRatio * target.GetComponent<Enemy>().maxHealth);
            Destroy(gameObject);
        }
        // 机关1
        else if(ID == SkillState.second && other.tag == "Organ1")
        {
            Destroy(other.gameObject);
        }
        // 机关2
        else if(ID == SkillState.third && other.tag == "Organ2")
        {
            Destroy(other.gameObject);
        }
    }
}