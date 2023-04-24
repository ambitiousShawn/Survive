using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalEnemy : MonoBehaviour
{
    // �����ٶ�
    public float speed = 5f;
    // �켣�뾶
    public float radius = 5f;
    // ��ת�ٶ�
    public float rotationSpeed = 60f;
    // Բ��λ��
    public Transform center;
    // ��Ҽ��뾶
    public float playerDetectRadius = 5f;
    // ͣ��ʱ��
    public float deathTimeThreshold = 3f;

    // ���нǶ�
    private float angle = 0;
    // �������ʱ��
    public float TimeSincePlayerEnteredRadius { get; set; }

    // ���
    public GameObject player;
    // ���
    public bool hide;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        hide = player.GetComponent<PlayerController>().isHide;
        // ��***���Ż�***��
        //animator.SetBool("fly", true);
        if (Detect())
        {
            // ���㶺��ʱ��
            TimeSincePlayerEnteredRadius += Time.deltaTime;
        }
        else
        {
            InitialTime();
        }

        // ���Ƶ�λ��
        Vector3 controlPoint = center.transform.position + new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
        // ��һλ��
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, controlPoint, speed * Time.deltaTime);
        
        // ��Բ����ת
        Vector3 direction = nextPosition - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        // ����λ��
        transform.position = nextPosition;

        // ���½Ƕ�
        angle += rotationSpeed * Time.deltaTime;
        angle = Mathf.Repeat(angle, 360f);
    }

    private bool Detect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerDetectRadius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player") && !hide)
            {
                // ����ʱ������
                if (TimeSincePlayerEnteredRadius >= deathTimeThreshold)
                {
                    collider.GetComponent<Player>().GoDie();
                }
                return true;
            }
        }
        return false;
    }

    private void InitialTime()
    {
        TimeSincePlayerEnteredRadius = 0f;
    }
}
