using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeAttack : MonoBehaviour
{
    // �����˺�ֵ
    public float baseDamage = 10f;
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

    // �Ƿ���й���
    private bool isAttacking = false;
    // ������Χ�����
    private SphereCollider attackCollider;

    public GameObject cameraPivot;
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
        if (player.GetComponent<PlayerController>().isMoving)
        {
            // �������������й���
            if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1) && !isAttacking)
            {
                isAttacking = true;

                var animator = GetComponent<Animator>();
                if (animator != null)
                {
                    // ��������
                    animator.SetTrigger("hit");

                    Invoke(nameof(DisableAttack), 1.0f);
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

    private void OnTriggerStay(Collider other)
    {
        if(!isAttacking)
        {
            return;
        }
        if(other.CompareTag("Enemy"))
        {
            // �˺��ܴ�С
            var damageAmount = baseDamage;

            // ���˳����˺�
            other.GetComponent<Health>().TakeDamage(damageAmount);

            // �����ܻ�����
            other.GetComponent<Animator>().SetTrigger("hit");

            // ���Զ���
            cameraPivot.GetComponent<CameraController>().ShakeCamera(5, 0.5f);

            Vector3 relative = other.transform.position - transform.position;
            Vector3 direction = new Vector3(relative.x, 0f, relative.z);

            // ���˻���Ч��
            other.GetComponent<Enemy>().KnockBack(direction * knockBackForce, knockBackTime);

            // ������Ч
            //Instantiate(hitEffectPerfab, other.ClosestPoint(transform.position), Quaternion.identity);
            //Destroy();
        }
    }

    private void DisableAttack()
    {
        isAttacking = false;
        attackCollider.enabled = false;
    }
}
