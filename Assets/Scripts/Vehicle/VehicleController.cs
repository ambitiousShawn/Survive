using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VehicleController : MonoBehaviour
{
    public GameObject vehicle;
    // ״̬��1Ϊ������0Ϊֹͣ
    private bool currentState = false;
    // �ƶ��ٶ�
    [SerializeField] private float speed = 5f;
    // ת���ٶ�
    [SerializeField] private float turnSpeed = 360;
    // �˺�
    [SerializeField] private float damage = 5f;
    // �ƶ�����
    private Vector3 input;
    // ����
    private Rigidbody vehicleRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        vehicleRigidBody = vehicle.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState)
        {
            GatherInput();
            Look();
        }
    }

    void FixedUpdate()
    {
        if (currentState)
        {
            // �ƶ�����˳��
            Move();
        }
    }

    // ��ȡ����
    void GatherInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    // ����ȷ����ǰ�ƶ������߼�
    void Look()
    {
        var animator = GetComponent<Animator>();
        if (input != Vector3.zero)
        {
            //animator.SetBool("walk", true);
            var relative = (transform.position + input.Toiso()) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
        else
        {
            //animator.SetBool("walk", false);
        }
    }

    // �ƶ�
    void Move()
    {
        vehicleRigidBody.MovePosition(transform.position + (transform.forward * input.magnitude) * speed * Time.deltaTime);
    }

    // ����ʱ���ã�ͨ������ȷ���Ƿ�����
    public void ChangeState(bool state)
    {
        currentState = state;
    }

    // ײ���;ö��½�
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            float velocityDamage = damage * vehicleRigidBody.velocity.magnitude;
            vehicle.GetComponent<VehicleSystem>().Damage(velocityDamage);
        }
    }
}
