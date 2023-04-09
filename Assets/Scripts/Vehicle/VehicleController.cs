using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VehicleController : MonoBehaviour
{
    public GameObject vehicle;
    // 状态：1为启动，0为停止
    private bool currentState = false;
    // 移动速度
    [SerializeField] private float speed = 5f;
    // 转向速度
    [SerializeField] private float turnSpeed = 360;
    // 伤害
    [SerializeField] private float damage = 5f;
    // 移动向量
    private Vector3 input;
    // 刚体
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
            // 移动更加顺滑
            Move();
        }
    }

    // 获取输入
    void GatherInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    // 朝向，确保当前移动符合逻辑
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

    // 移动
    void Move()
    {
        vehicleRigidBody.MovePosition(transform.position + (transform.forward * input.magnitude) * speed * Time.deltaTime);
    }

    // 交互时调用，通过按键确定是否启动
    public void ChangeState(bool state)
    {
        currentState = state;
    }

    // 撞击耐久度下降
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            float velocityDamage = damage * vehicleRigidBody.velocity.magnitude;
            vehicle.GetComponent<VehicleSystem>().Damage(velocityDamage);
        }
    }
}
