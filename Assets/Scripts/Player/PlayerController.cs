using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    // 跳跃力度
    public float jumpForce = 10f;
    // 最大力量
    public float maxPower = 10f;
    // 增长速率
    public float powerIncreaseRate = 1.5f;
    // 是否蓄力
    private bool isCharging = false;
    // 当前力量
    private float currentPower;
    // 检测是否接触地面
    public bool isGrounded;

    // 角色控制
    public bool isMoving = true;
    // 躲藏信息
    public bool isHide = false;

    // 刚体
    [SerializeField] private Rigidbody rb;
    // 移动速度
    [SerializeField] private float speed = 5f;
    // 初始速度
    [SerializeField] private float originalSpeed;
    // 转向速度
    [SerializeField] private float turnSpeed = 360;
    // 移动向量
    private Vector3 input;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            CheckGrounded();
            GatherInput();
            Look();
            if (isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartChargeJump();
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    ReleaseJump();
                }
                if (isCharging)
                {
                    ChargeJump();
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            // 移动更加顺滑
            Move();
        }
    }

    // 获取输入
    void GatherInput()
    {
        input = new Vector3(-Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
    }

    // 朝向，确保当前移动符合逻辑
    void Look()
    {
        var animator = GetComponent<Animator>();
        if (input != Vector3.zero)
        {
            animator.SetBool("walk", true);
            var relative = (transform.position + input.Toiso()) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("walk", false);
        }
    }

    // 移动
    void Move()
    {
        rb.MovePosition(transform.position + (transform.forward * input.magnitude) * speed * Time.deltaTime);
    }

    // debuff减速
    public void Slow(float duration, float slowedSpeed)
    {
        StartCoroutine(SlowCoroutine(duration, slowedSpeed));
        // 将敌人速度设置为slowedSpeed

        IEnumerator SlowCoroutine(float duration, float slowedSpeed)
        {
            speed = slowedSpeed;

            // 等待指定的持续时间
            yield return new WaitForSeconds(duration);

            // 恢复敌人原始速度
            speed = originalSpeed;
        }
    }

    // 检测是否接触地面
    void CheckGrounded()
    {
        RaycastHit hit;
        Vector3 positon = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        // 这部分射线长度值值确定
        isGrounded = Physics.Raycast(positon, Vector3.down, out hit, 0.3f);
    }
    // 开始蓄力跳跃
    void StartChargeJump()
    {
        isCharging = true;
        currentPower = 0;
    }
    // 蓄力
    void ChargeJump()
    {
        if (currentPower < maxPower)
        {
            currentPower += Time.deltaTime * powerIncreaseRate;
            isCharging = false;
        }
    }
    // 跳跃
    void ReleaseJump()
    {
        isCharging = false;
        rb.velocity = new Vector3(rb.velocity.x, jumpForce + currentPower, rb.velocity.y);
        currentPower = 0;
    }

    // 躲藏
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
}
