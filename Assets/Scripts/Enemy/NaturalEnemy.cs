using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalEnemy : MonoBehaviour
{
    // 飞行速度
    public float speed = 5f;
    // 轨迹半径
    public float radius = 5f;
    // 旋转速度
    public float rotationSpeed = 60f;
    // 圆心位置
    public Transform center;
    // 玩家检测半径
    public float playerDetectRadius = 5f;
    // 停留时长
    public float deathTimeThreshold = 3f;

    // 飞行角度
    private float angle = 0;
    // 进入检测的时间
    [SerializeField] private float timeSincePlayerEnteredRadius;

    // 玩家
    public GameObject player;
    // 躲藏
    public bool hide;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        hide = player.GetComponent<PlayerController>().isHide;
        // （***待优化***）
        //animator.SetBool("fly", true);
        Detect();

        // 控制点位置
        Vector3 controlPoint = center.transform.position + new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
        // 下一位置
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, controlPoint, speed * Time.deltaTime);
        
        // 沿圆弧旋转
        Vector3 direction = nextPosition - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        // 更新位置
        transform.position = nextPosition;

        // 更新角度
        angle += rotationSpeed * Time.deltaTime;
        angle = Mathf.Repeat(angle, 360f);
    }

    private void Detect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerDetectRadius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player") && !hide)
            {
                // 计算逗留时间
                timeSincePlayerEnteredRadius += Time.deltaTime;
                // 超过时长死亡
                if (timeSincePlayerEnteredRadius >= deathTimeThreshold)
                {
                    collider.GetComponent<Player>().GoDie();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hide)
        {
            // 进入重置逗留时间
            timeSincePlayerEnteredRadius = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeSincePlayerEnteredRadius = Mathf.Infinity;
        }
    }
}
