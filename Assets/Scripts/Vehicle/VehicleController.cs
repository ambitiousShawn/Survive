using System.Collections;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public GameObject vehicle;
    // 状态：1为启动，0为停止
    // 具体状态机也行，简化
    private bool currentState = false;
    // 移动速度
    [SerializeField] private float speed = 5f;
    // 转向速度
    [SerializeField] private float turnSpeed = 360;
    // 撞击障碍伤害
    [SerializeField] private float damage = 5f;

    // 刚体
    private Rigidbody vehicleRigidBody;
    // 相机轴
    [SerializeField] private Transform cameraPivot;

    // 移动输入
    private float vertical;
    private float horizontal;

    // 施加力范围
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
            // 移动更加顺滑
            Move();
        }
    }

    // 获取输入
    void GatherInput()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    // 朝向，确保当前移动符合逻辑
    void Look()
    {
        if (vertical != 0 || horizontal != 0)
        {
            var relative = (cameraPivot.forward * vertical + cameraPivot.right * horizontal);
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
    }

    // 移动
    void Move()
    {
        float input = new Vector3(vertical, 0, horizontal).magnitude;
        vehicleRigidBody.MovePosition(transform.position + (transform.forward * input) * speed * Time.deltaTime);
    }

    // 交互时调用，通过按键确定是否启动
    public void ChangeState(bool state)
    {
        currentState = state;
    }

    // 虚拟风
    private void AddWind()
    {
        // 随机刮风
        float wind = Random.value;
        if (wind < 0.5f)
        {
            // 随机力
            Vector3 Force = new Vector3(Random.Range(minForce, maxForce), 0, Random.Range(minForce, maxForce));
            StartCoroutine(Wind());
            IEnumerator Wind()
            {
                vehicleRigidBody.AddForce(Force, ForceMode.Impulse);
                yield return new WaitForSeconds(10f);
            }
        }
    }

    // 撞击耐久度下降
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // 撞击障碍物耐久度下降
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
            // 撞击敌人停止
            vehicleRigidBody.velocity = Vector3.zero;
        }
    }
}
