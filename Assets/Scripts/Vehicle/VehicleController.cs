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

    // ʩ������Χ
    [SerializeField] private float minForce = 0f;
    [SerializeField] private float maxForce = 1f;

    // Start is called before the first frame update
    void Start()
    {
        vehicleRigidBody = vehicle.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        AddWind();

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
        input = new Vector3(-Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));
    }

    // ����ȷ����ǰ�ƶ������߼�
    void Look()
    {
        if (input != Vector3.zero)
        {
            var relative = (transform.position + input.Toiso()) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
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

    // �����
    private void AddWind()
    {
        // ����η�
        float wind = Random.value;
        if (wind < 0.5f)
        {
            // �����
            Vector3 Force = new Vector3(Random.Range(minForce, maxForce), 0, Random.Range(minForce, maxForce));
            StartCoroutine(Wind());
            IEnumerator Wind()
            {
                vehicleRigidBody.AddForce(Force, ForceMode.Impulse);
                yield return new WaitForSeconds(5f);
            }
        }
    }

    // ײ���;ö��½�
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // ײ���ϰ����;ö��½�
            float velocityDamage = damage * vehicleRigidBody.velocity.magnitude;
            StartCoroutine(TakeDamage());
            IEnumerator TakeDamage()
            {
                vehicle.GetComponent<VehicleSystem>().Damage(velocityDamage);
                yield return new WaitForSeconds(1f);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // ײ������ֹͣ
            vehicleRigidBody.velocity = Vector3.zero;
        }
        else
        {
            vehicleRigidBody.velocity = Vector3.zero;
        }
    }
}
