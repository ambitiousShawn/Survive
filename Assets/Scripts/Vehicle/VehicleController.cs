using System.Collections;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public GameObject vehicle;
    // ״̬��1Ϊ������0Ϊֹͣ
    // ����״̬��Ҳ�У���
    private bool currentState = false;
    // �ƶ��ٶ�
    [SerializeField] private float speed = 5f;
    // ת���ٶ�
    [SerializeField] private float turnSpeed = 360;
    // ײ���ϰ��˺�
    [SerializeField] private float damage = 5f;

    // ����
    private Rigidbody vehicleRigidBody;
    // �����
    [SerializeField] private Transform cameraPivot;

    // �ƶ�����
    private float vertical;
    private float horizontal;

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
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    // ����ȷ����ǰ�ƶ������߼�
    void Look()
    {
        if (vertical != 0 || horizontal != 0)
        {
            var relative = (cameraPivot.forward * vertical + cameraPivot.right * horizontal);
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
    }

    // �ƶ�
    void Move()
    {
        float input = new Vector3(vertical, 0, horizontal).magnitude;
        vehicleRigidBody.MovePosition(transform.position + (transform.forward * input) * speed * Time.deltaTime);
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
                yield return new WaitForSeconds(10f);
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
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("MeleeEnemy"))
        {
            // ײ������ֹͣ
            vehicleRigidBody.velocity = Vector3.zero;
        }
    }
}
