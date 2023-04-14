using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    // ��Ծ����
    public float jumpForce = 10f;
    // �������
    public float maxPower = 10f;
    // ��������
    public float powerIncreaseRate = 1.5f;
    // ��ǰ����
    private float currentPower;
    // ����Ƿ�Ӵ�����
    public bool isGrounded;

    // ��ɫ����
    public bool isMoving = true;
    // �����Ϣ
    public bool isHide = false;
    // ��Ծ
    private bool isJumping = false;

    // ����
    [SerializeField] private Rigidbody rb;
    // �ƶ��ٶ�
    [SerializeField] private float speed = 5f;
    // ��ʼ�ٶ�
    [SerializeField] private float originalSpeed;
    // ת���ٶ�
    [SerializeField] private float turnSpeed = 360;
    // �ƶ�����
    private Vector3 input;

    private Animator animator;
    // �����ƶ�
    [SerializeField] private Transform anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
                rb.freezeRotation = false;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetBool("fly", true);
                    StartChargeJump();
                }
            }
            else
            {
                rb.freezeRotation = true;
            }

            if(isJumping && Input.GetKeyUp(KeyCode.Space))
            {
                ChargeJump();
                StartCoroutine(ReleaseJump());
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            // �ƶ�����˳��
            Move();
        }
    }

    // ��ȡ����
    void GatherInput()
    {
        input = new Vector3(-Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
    }

    // ����ȷ����ǰ�ƶ������߼�
    void Look()
    {
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

    // �ƶ�
    void Move()
    {
        rb.MovePosition(transform.position + (transform.forward * input.magnitude) * speed * Time.deltaTime);
    }

    // debuff����
    public void Slow(float duration, float slowedSpeed)
    {
        StartCoroutine(SlowCoroutine(duration, slowedSpeed));
        // �������ٶ�����ΪslowedSpeed

        IEnumerator SlowCoroutine(float duration, float slowedSpeed)
        {
            speed = slowedSpeed;

            // �ȴ�ָ���ĳ���ʱ��
            yield return new WaitForSeconds(duration);

            // �ָ�����ԭʼ�ٶ�
            speed = originalSpeed;
        }
    }

    // ����Ƿ�Ӵ�����
    void CheckGrounded()
    {
        RaycastHit hit;
        Vector3 positon = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        // �ⲿ�����߳���ֵֵȷ��
        isGrounded = Physics.Raycast(positon, Vector3.down, out hit, 0.3f);
    }
    // ��ʼ������Ծ
    void StartChargeJump()
    {
        isJumping = true;
        currentPower = 0;
        rb.useGravity = false;
    }
    // ����
    void ChargeJump()
    {
        if (currentPower < maxPower)
        {
            currentPower += powerIncreaseRate;
        }
        else
        {
            StartCoroutine(ReleaseJump());
        }
    }
    // ��Ծ
    IEnumerator ReleaseJump()
    {
        isJumping = false;
        animator.SetBool("fly", false);
        rb.useGravity = true;
        currentPower = 0;
        transform.position = anim.position;
        yield return new WaitForSeconds(5f);
    }

    // ���
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
