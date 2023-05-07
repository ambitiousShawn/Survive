using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attack;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;

public class Projectile : MonoBehaviour
{
    [SerializeField] public SkillState ID;

    // Ͷ������ٶ�
    public Vector3 velocity;
    // ÿ���˺�
    public float damageRatio;
    // debuff����ʱ��
    public float duration = 5f;

    private Rigidbody rb;
    private Collider target;

    // �洢�������
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
            // ���������
            transform.LookAt(target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 2f);

            // TODO��ί��debuff
            //attack.doEffect();
            target.gameObject.GetComponent<Enemy>().TakeDamage(damageRatio * target.GetComponent<Enemy>().maxHealth);
            Destroy(gameObject);
        }
        // ����1
        else if(ID == SkillState.second && other.tag == "Organ1")
        {
            Destroy(other.gameObject);
        }
        // ����2
        else if(ID == SkillState.third && other.tag == "Organ2")
        {
            Destroy(other.gameObject);
        }
    }
}