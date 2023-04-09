using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    public Rigidbody body;
    // 用来获取正向
    public Transform launchPoint;

    public bool isClimbing;
    // 攀爬调整视角
    public bool onWall;

    // 目标位置
    private Vector3 targetPos;

    // 射线检测距离
    [SerializeField] private float wallRayLength = 1;

    // 偏移
    [SerializeField]private float wallOffset = 1f;
    public Animator animator;

    // 攀爬输入
    private Vector3 input;

    // 爬墙移动速度
    [SerializeField] private float climbSpeed = 5f;

    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 检测是否达到攀爬距离
        CheckClimb();
        if(isClimbing && !onWall)
        {
            SetBodyPositionToWall();
        }
        else if(onWall)
        {
            // 修正距离
            FixBodyPos();
            // 移动
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

        // 正向
        Vector3 dir = new Vector3(relative.x, 0, relative.z);
        RaycastHit hit;

        // 靠近可攀爬
        Debug.DrawLine(origin, origin + dir, Color.green);
        if (Physics.Raycast(origin, dir, out hit, wallRayLength))
        {
            // 如果是墙体，可攀爬
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

    // 初始化
    private void InitClimb(RaycastHit hit)
    {
        isClimbing = true;
        onWall = false;
        targetPos = hit.point + hit.normal * wallOffset;

        // 攀爬动画
        // animator.CrossFade("climb",0.2f);
        Debug.Log("Hit Wall");
    }

    // 上墙移动
    private void SetBodyPositionToWall()
    {
        // 到达墙位置，在墙上，距离和位置有关（待调整）
        if(Vector3.Distance(transform.position, targetPos) < 0)
        {
            onWall = true;
            // 保持静止
            body.useGravity = false;
            transform.position = targetPos;
            return;
        }
        // 插值靠近
        Vector3 lerpTargetPos = Vector3.MoveTowards(transform.position, targetPos, 0.2f);
        transform.position = lerpTargetPos;
    }

    // 人物移动和攀爬移动切换（***待解决***）
    private void Move()
    {
        GatherInput();
        transform.position = transform.position + input.normalized * climbSpeed * Time.deltaTime;
    }

    // 在墙上进行位置修正（***待修正***）
    private void FixBodyPos()
    {
        // 倾斜物体攀爬
    }

    // 获取输入
    void GatherInput()
    {
        input = Input.GetAxisRaw("Horizontal") * transform.right + Input.GetAxisRaw("Vertical") * transform.up;
    }
}
