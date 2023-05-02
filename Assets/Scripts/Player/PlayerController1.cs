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
    // ��ǰ���״̬
    private PlayerState currentState;

    [Header("�ٶ���ز���")]
    // �ƶ��ٶ�
    [SerializeField] private float speed = 5f;
    // TODO����Ծ���Ż�
    [SerializeField] private float flySpeed = 1f;
    // ��ʼ�ٶ�
    [SerializeField] private float originalSpeed;
    // ת���ٶ�
    [SerializeField] private float turnSpeed = 360;

    // �ƶ�����
    private float vertical;
    private float horizontal;

    // ����Ƿ�Ӵ�����
    [SerializeField] private bool isGrounded;
    // ��ɫ���ƣ������ؾ߽���
    public bool isMoving = true;
    // �����Ϣ
    public bool isHide = false;

    // �����
    [SerializeField] private Transform cameraPivot;
    // ����
    private Rigidbody rb;

    private Animator animator;
    // �����ƶ� TODO���Ż�
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
        // ���Ϊ��ҿ���
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
                // �ڿ���ʱ������ת
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
    // @brief       ��Ϊ
    //-----------------------------------------------------

    // ��ȡ����
    void GatherInput()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
    }
    // ����ȷ����ǰ�ƶ������߼�
    void Look()
    {
        // ������������˶�
        Vector3 relative = (cameraPivot.forward * vertical + cameraPivot.right * horizontal);
        Quaternion rot = Quaternion.LookRotation(relative, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
    }
    // �ƶ�
    void Move()
    {
        GatherInput();
        Look();
        float direction = new Vector3(vertical, 0, horizontal).magnitude;
        rb.MovePosition(transform.position + (transform.forward * direction) * speed * Time.deltaTime);
    }

    // ��ʼ������Ծ
    void StartChargeJump()
    {
        rb.useGravity = false;
    }
    // ����
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
    // ��Ծ
    IEnumerator ReleaseJump()
    {
        rb.useGravity = true;
        transform.position = anim.position;
        timer = 0;
        yield return new WaitForSeconds(5f);
        ChangeState(PlayerState.Idle);
    }
    // ��Ծ
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

    // debuff����
    public void Slow(float duration, float slowRatio)
    {
        StartCoroutine(SlowCoroutine());
        // �������ٶ�����ΪslowedSpeed

        IEnumerator SlowCoroutine()
        {
            speed = originalSpeed * slowRatio;

            // �ȴ�ָ���ĳ���ʱ��
            yield return new WaitForSeconds(duration);

            // �ָ�����ԭʼ�ٶ�
            speed = originalSpeed;
        }
    }

    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.1
    // @brief       ���
    //-----------------------------------------------------

    // �ı�״̬
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
    // ������
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    // ���������
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
    // @brief       �����ӽ�����ҿ���
    //-----------------------------------------------------
    // ��Ծ����
    //public float jumpForce = 10f;

    //// ����Ƿ�Ӵ�����
    //public bool isGrounded;

    //// ��ɫ����
    //public bool isMoving = true;
    //// �����Ϣ
    //public bool isHide = false;
    //// ��Ծ
    //private bool isJumping = false;

    //// ����
    //[SerializeField] private Rigidbody rb;
    //// �ƶ��ٶ�
    //[SerializeField] private float speed = 5f;

    //[SerializeField] private float flySpeed = 1f;
    //// ��ʼ�ٶ�
    //[SerializeField] private float originalSpeed;
    //// ת���ٶ�
    //[SerializeField] private float turnSpeed = 360;
    //// �ƶ�����
    //private Vector3 input;

    //private Animator animator;
    //// �����ƶ�
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
    //        // �ƶ�����˳��
    //        Move();
    //    }
    //}

    //// ��ȡ����
    //void GatherInput()
    //{
    //    input = new Vector3(-Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
    //}

    //// ����ȷ����ǰ�ƶ������߼�
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

    //// �ƶ�
    //void Move()
    //{
    //    rb.MovePosition(transform.position + (transform.forward * input.magnitude) * speed * Time.deltaTime);
    //}

    //// debuff����
    //public void Slow(float duration, float slowRatio)
    //{
    //    StartCoroutine(SlowCoroutine());
    //    // �������ٶ�����ΪslowedSpeed

    //    IEnumerator SlowCoroutine()
    //    {
    //        speed = originalSpeed * slowRatio;

    //        // �ȴ�ָ���ĳ���ʱ��
    //        yield return new WaitForSeconds(duration);

    //        // �ָ�����ԭʼ�ٶ�
    //        speed = originalSpeed;
    //    }
    //}

    //// ����Ƿ�Ӵ�����
    //void CheckGrounded()
    //{
    //    RaycastHit hit;
    //    Vector3 positon = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
    //    // �ⲿ�����߳���ֵֵȷ��
    //    isGrounded = Physics.Raycast(positon, Vector3.down, out hit, 0.3f);
    //}
    //// ��ʼ������Ծ
    //void StartChargeJump()
    //{
    //    isJumping = true;
    //    rb.useGravity = false;
    //}
    //// ����
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
    //// ��Ծ
    //IEnumerator ReleaseJump()
    //{
    //    isJumping = false;
    //    animator.SetBool("fly", false);
    //    rb.useGravity = true;
    //    transform.position = anim.position;
    //    timer = 0;
    //    yield return new WaitForSeconds(5f);
    //}

    //// ���
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
