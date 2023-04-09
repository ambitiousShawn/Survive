using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    public Rigidbody body;
    // ������ȡ����
    public Transform launchPoint;

    public bool isClimbing;
    // ���������ӽ�
    public bool onWall;

    // Ŀ��λ��
    private Vector3 targetPos;

    // ���߼�����
    [SerializeField] private float wallRayLength = 1;

    // ƫ��
    [SerializeField]private float wallOffset = 1f;
    public Animator animator;

    // ��������
    private Vector3 input;

    // ��ǽ�ƶ��ٶ�
    [SerializeField] private float climbSpeed = 5f;

    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ����Ƿ�ﵽ��������
        CheckClimb();
        if(isClimbing && !onWall)
        {
            SetBodyPositionToWall();
        }
        else if(onWall)
        {
            // ��������
            FixBodyPos();
            // �ƶ�
            Move();
        }
        else
        {
            body.useGravity = true;
        }
    }

    private bool CheckClimb()
    {
        Vector3 origin = transform.position;
        Vector3 relative = transform.forward;

        // ����
        Vector3 dir = new Vector3(relative.x, 0, relative.z);
        RaycastHit hit;

        // ����������
        Debug.DrawLine(origin, origin + dir, Color.green);
        if (Physics.Raycast(origin, dir, out hit, wallRayLength))
        {
            // �����ǽ�壬������
            if (hit.collider.CompareTag("Wall"))
            {
                InitClimb(hit);
                return true;
            }
        }
        isClimbing = false;
        onWall = false;
        return false;
    }

    // ��ʼ��
    private void InitClimb(RaycastHit hit)
    {
        isClimbing = true;
        onWall = false;
        targetPos = hit.point + hit.normal * wallOffset;

        // ��������
        // animator.CrossFade("climb",0.2f);
        Debug.Log("Hit Wall");
    }

    // ��ǽ�ƶ�
    private void SetBodyPositionToWall()
    {
        // ����ǽλ�ã���ǽ�ϣ������λ���йأ���������
        if(Vector3.Distance(transform.position, targetPos) < 0)
        {
            onWall = true;
            // ���־�ֹ
            body.useGravity = false;
            transform.position = targetPos;
            return;
        }
        // ��ֵ����
        Vector3 lerpTargetPos = Vector3.MoveTowards(transform.position, targetPos, 0.2f);
        transform.position = lerpTargetPos;
    }

    // �����ƶ��������ƶ��л���***�����***��
    private void Move()
    {
        GatherInput();
        transform.position = transform.position + input.normalized * climbSpeed * Time.deltaTime;
    }

    // ��ǽ�Ͻ���λ��������***������***��
    private void FixBodyPos()
    {
        // ��б��������
    }

    // ��ȡ����
    void GatherInput()
    {
        input = Input.GetAxisRaw("Horizontal") * transform.right + Input.GetAxisRaw("Vertical") * transform.up;
    }
}
