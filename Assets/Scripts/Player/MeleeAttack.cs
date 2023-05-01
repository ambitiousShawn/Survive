using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeAttack : MonoBehaviour
{
    // �˺�����
    public float damageRatio = 0.3f;
    // ������Χ
    public float attackRange = 1f;
    // ��������
    public float knockBackForce = 10f;
    // ����ʱ��
    public float knockBackTime = 1f;
    // ������ЧԤ�Ƽ�
    public GameObject hitEffectPerfab;
    // ��Ч����ʱ��
    public float hitEffectDuration = 1f;
    // �������
    [SerializeField] private float attackInterval;

    // �Ƿ���й���
    private bool isAttacking = false;
    // ������Χ�����
    private SphereCollider attackCollider;
    public GameObject player;

    private void Start()
    {
        attackCollider = gameObject.AddComponent<SphereCollider>();
        attackCollider.isTrigger = true;
        attackCollider.radius = attackRange;
        // ��ʼ������
        attackCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // �������ҿ��Ʋſ��Խ�ս����
        if (player.GetComponent<PlayerController>().isMoving)
        {
            // �������������й����Ҳ��ڹ���cd��
            // ���ֽ�սԶ����Ҫ�ж�����Ҽ�״̬
            if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1) && !isAttacking)
            {
                isAttacking = true;

                var animator = GetComponent<Animator>();
                if (animator != null)
                {
                    // ��������
                    animator.SetTrigger("hit");

                    // �������
                    StartCoroutine(DisableAttack());
                }

                //������ײ��
                attackCollider.enabled = true;
            }
            else
            {
                isAttacking = false;
            }
        }
    }

    IEnumerator DisableAttack()
    {
        isAttacking = false;
        attackCollider.enabled = false;
        yield return new WaitForSeconds(attackInterval);
    }

    // ��ս��Χ���˼��
    private void OnTriggerStay(Collider other)
    {
        if(!isAttacking)
        {
            return;
        }
        // Ŀǰֻ�ɹ�����ս����
        if(other.CompareTag("MeleeEnemy"))
        {
            // �˺��ܴ�С���������ֵ�ı����˺�
            var damageAmount = damageRatio * other.GetComponent<Health>().maxHealth;

            // ���˳����˺�
            other.GetComponent<Health>().TakeDamage(damageAmount);

            // �����ܻ�����
            other.GetComponent<Animator>().SetTrigger("hit");

            Vector3 relative = other.transform.position - transform.position;
            Vector3 direction = new Vector3(relative.x, 0f, relative.z);

            // ���˻���Ч��
            other.GetComponent<Enemy>().KnockBack(direction * knockBackForce, knockBackTime);

            // ������Ч
            //Instantiate(hitEffectPerfab, other.ClosestPoint(transform.position), Quaternion.identity);
            //Destroy();
        }
    }
}
