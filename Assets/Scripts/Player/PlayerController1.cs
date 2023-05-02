using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using UnityEngine.XR;

public enum PlayerState
{
    Idle,
    Move,
    Jump
}

public class PlayerController1 : MonoBehaviour
{
    // 当前玩家状态
    private PlayerState currentState;

    [Header("速度相关参数")]
    // 移动速度
    [SerializeField] private float speed = 5f;
    // TODO：跳跃需优化
    [SerializeField] private float flySpeed = 1f;
    // 初始速度
    [SerializeField] private float originalSpeed;
    // 转向速度
    [SerializeField] private float turnSpeed = 360;

    // 移动输入
    private float vertical;
    private float horizontal;

    // 检测是否接触地面
    [SerializeField] private bool isGrounded;
    // 角色控制，用于载具交互
    public bool isMoving = true;
    // 躲藏信息
    public bool isHide = false;

    // 相机轴
    [SerializeField] private Transform cameraPivot;
    // 刚体
    private Rigidbody rb;

    private Animator animator;
    // 动画移动 TODO：优化
    [SerializeField] private Transform anim;
    [SerializeField] private float maxTime = 2f;
    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        originalSpeed = speed;
        currentState = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        // 如果为玩家控制
        if (isMoving)
        {
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                ChangeState(PlayerState.Jump);
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                ChangeState(PlayerState.Move);
            }
            else
            {
                ChangeState(PlayerState.Idle);
            }

            if (isGrounded)
            {
                // 在空中时锁定旋转
                rb.freezeRotation = false;
            }
            else
            {
                rb.freezeRotation = true;
            }
        }
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                animator.SetBool("walk", false);
                break;
            case PlayerState.Jump:
                animator.SetBool("fly", true);
                Jump();
                break;
            case PlayerState.Move:
                animator.SetBool("walk", true);
                Move();
                break;
            default:
                break;
        }
    }

    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.1
    // @brief       行为
    //-----------------------------------------------------

    // 获取输入
    void GatherInput()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
    }
    // 朝向，确保当前移动符合逻辑
    void Look()
    {
        // 根据相机正向运动
        Vector3 relative = (cameraPivot.forward * vertical + cameraPivot.right * horizontal);
        Quaternion rot = Quaternion.LookRotation(relative, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
    }
    // 移动
    void Move()
    {
        GatherInput();
        Look();
        float direction = new Vector3(vertical, 0, horizontal).magnitude;
        rb.MovePosition(transform.position + (transform.forward * direction) * speed * Time.deltaTime);
    }

    // 开始蓄力跳跃
    void StartChargeJump()
    {
        rb.useGravity = false;
    }
    // 蓄力
    void ChargeJump()
    {
        if (timer < maxTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, anim.position, flySpeed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(ReleaseJump());
        }
    }
    // 跳跃
    IEnumerator ReleaseJump()
    {
        rb.useGravity = true;
        transform.position = anim.position;
        timer = 0;
        yield return new WaitForSeconds(5f);
        ChangeState(PlayerState.Idle);
    }
    // 跳跃
    void Jump()
    {
        StartChargeJump();
        ChargeJump();
    }

    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.1
    // @brief       debuff
    //-----------------------------------------------------

    // debuff减速
    public void Slow(float duration, float slowRatio)
    {
        StartCoroutine(SlowCoroutine());
        // 将敌人速度设置为slowedSpeed

        IEnumerator SlowCoroutine()
        {
            speed = originalSpeed * slowRatio;

            // 等待指定的持续时间
            yield return new WaitForSeconds(duration);

            // 恢复敌人原始速度
            speed = originalSpeed;
        }
    }

    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.1
    // @brief       检测
    //-----------------------------------------------------

    // 改变状态
    void ChangeState(PlayerState newState)
    {
        if (currentState == newState)
        {
            return;
        }
        else
        {
            currentState = newState;
        }
    }
    // 地面检测
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    // 躲藏与死域
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Grass":
                isHide = true;
                break;
            case "DeathZone":
                gameObject.GetComponent<Player>().GoDie();
                break;
            default: break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grass"))
        {
            isHide = false;
        }
    }

    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.0
    // @brief       正交视角下玩家控制
    //-----------------------------------------------------
    // 跳跃力度
    //public float jumpForce = 10f;

    //// 检测是否接触地面
    //public bool isGrounded;

    //// 角色控制
    //public bool isMoving = true;
    //// 躲藏信息
    //public bool isHide = false;
    //// 跳跃
    //private bool isJumping = false;

    //// 刚体
    //[SerializeField] private Rigidbody rb;
    //// 移动速度
    //[SerializeField] private float speed = 5f;

    //[SerializeField] private float flySpeed = 1f;
    //// 初始速度
    //[SerializeField] private float originalSpeed;
    //// 转向速度
    //[SerializeField] private float turnSpeed = 360;
    //// 移动向量
    //private Vector3 input;

    //private Animator animator;
    //// 动画移动
    //[SerializeField] private Transform anim;
    //[SerializeField] private float maxTime = 2f;
    //[SerializeField] private float timer;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    animator = GetComponent<Animator>();
    //    originalSpeed = speed;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (isMoving)
    //    {
    //        CheckGrounded();
    //        GatherInput();
    //        Look();
    //        if (isGrounded)
    //        {
    //            rb.freezeRotation = false;
    //            if (Input.GetKeyDown(KeyCode.Space))
    //            {
    //                animator.SetBool("fly", true);
    //                StartChargeJump();
    //            }
    //        }
    //        else
    //        {
    //            rb.freezeRotation = true;
    //        }

    //        if (isJumping)
    //        {
    //            ChargeJump();
    //        }
    //    }
    //}

    //void FixedUpdate()
    //{
    //    if (isMoving)
    //    {
    //        // 移动更加顺滑
    //        Move();
    //    }
    //}

    //// 获取输入
    //void GatherInput()
    //{
    //    input = new Vector3(-Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
    //}

    //// 朝向，确保当前移动符合逻辑
    //void Look()
    //{
    //    if (input != Vector3.zero)
    //    {
    //        animator.SetBool("walk", true);
    //        var relative = (transform.position + input.Toiso()) - transform.position;
    //        var rot = Quaternion.LookRotation(relative, Vector3.up);

    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
    //    }
    //    else
    //    {
    //        animator.SetBool("walk", false);
    //    }
    //}

    //// 移动
    //void Move()
    //{
    //    rb.MovePosition(transform.position + (transform.forward * input.magnitude) * speed * Time.deltaTime);
    //}

    //// debuff减速
    //public void Slow(float duration, float slowRatio)
    //{
    //    StartCoroutine(SlowCoroutine());
    //    // 将敌人速度设置为slowedSpeed

    //    IEnumerator SlowCoroutine()
    //    {
    //        speed = originalSpeed * slowRatio;

    //        // 等待指定的持续时间
    //        yield return new WaitForSeconds(duration);

    //        // 恢复敌人原始速度
    //        speed = originalSpeed;
    //    }
    //}

    //// 检测是否接触地面
    //void CheckGrounded()
    //{
    //    RaycastHit hit;
    //    Vector3 positon = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
    //    // 这部分射线长度值值确定
    //    isGrounded = Physics.Raycast(positon, Vector3.down, out hit, 0.3f);
    //}
    //// 开始蓄力跳跃
    //void StartChargeJump()
    //{
    //    isJumping = true;
    //    rb.useGravity = false;
    //}
    //// 蓄力
    //void ChargeJump()
    //{
    //    if (timer < maxTime)
    //    {
    //        timer += Time.deltaTime;
    //        transform.position = Vector3.MoveTowards(transform.position, anim.position, flySpeed * Time.deltaTime);
    //    }
    //    else
    //    {
    //        StartCoroutine(ReleaseJump());
    //    }
    //}
    //// 跳跃
    //IEnumerator ReleaseJump()
    //{
    //    isJumping = false;
    //    animator.SetBool("fly", false);
    //    rb.useGravity = true;
    //    transform.position = anim.position;
    //    timer = 0;
    //    yield return new WaitForSeconds(5f);
    //}

    //// 躲藏
    //private void OnTriggerEnter(Collider other)
    //{
    //    switch (other.tag)
    //    {
    //        case "Grass":
    //            isHide = true;
    //            break;
    //        case "DeathZone":
    //            gameObject.GetComponent<Player>().GoDie();
    //            break;
    //        default: break;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Grass"))
    //    {
    //        isHide = false;
    //    }
    //}
}
