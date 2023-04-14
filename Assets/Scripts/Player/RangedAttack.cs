using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;

public class RangedAttack : MonoBehaviour
{
    public float fluidConsume = 10f;
    public float rotationSpeed = 5f;

    public Camera mainCamera;
    public GameObject follower;
    public GameObject bug;

    // 投射物预制体
    public GameObject[] projectilePerfab;
    // 投射物发射点
    public Transform launchPoint;
    // 发射方向
    private Vector3 launchDirection;
    // 目标位置
    private Vector3 targetPosition;
    // 发射点和Player来确定正向
    private Vector3 relative;

    // 指针较远才可转向，增强可玩性
    [SerializeField] private float distance = 1f;

    // 相机正向
    private Vector3 offset;
    // 渲染宽度
    public float width = 1f;

    // 每秒伤害
    public float damagePerSecond = 5f;
    // 减速时间
    public float slowDuration = 2f;

    private bool isAiming = false;
    private bool isShooting = false;

    // 根据蓄力时间来确定抛射速度
    [SerializeField] private float forceMultiplier = 10f;
    // 最大蓄力时间
    [SerializeField] private float maxChargeTime = 3f;
    // 轨迹预测步长
    [SerializeField] private float trajectoryPredicitionTimeStep = 0.1f;
    // 轨迹点最大数量
    [SerializeField] private int maxTrajectoryPointCount = 100;
    // 当前蓄力时间
    private float chargeTime = 0f;
    // 当前是否蓄力
    private bool isChargingShotEnabled = false;
    // 当前体液值
    private float fluidValue;
    // 记录预制体初速度
    private Vector3 velocity;

    // 技能切换
    [SerializeField] enum skill { first, second, third };

    private int currentSkillID = (int)skill.first;
    // 切换数据，只需要判断是第几技能，根据技能 ID 来确定伤害和子弹（***待实现***）
    // 类似载具状态判定，预制体切换

    private IEnumerator ShootProjectile()
    {
        isShooting = true;
        while (isShooting)
        {
            GameObject projectileInstance = Instantiate(projectilePerfab[currentSkillID], launchPoint.position, Quaternion.identity);
            Projectile projectile = projectileInstance.GetComponent<Projectile>();

            if (projectile != null)
            {
                projectile.velocity = velocity;
                projectile.damagePerSecond = damagePerSecond;
                projectile.slowDuration = slowDuration;
                projectile.launchDirection = launchDirection;
                Destroy(projectileInstance, 5f);
            }
            yield return new WaitForSeconds(2.5f);
        }
    }

    private void FixedUpdate()
    {
        if (bug.GetComponent<PlayerController>().isMoving)
        {
            if (Input.GetMouseButton(1))
            {
                isAiming = true;
                GetTargetPosition();

                // 转向
                Vector3 lookDirection = targetPosition - bug.transform.position;

                // 确保只在水平面内旋转
                Vector3 look = new Vector3(lookDirection.x, 0, lookDirection.z);

                // 指针较远时才可以旋转
                if (look.magnitude > distance)
                {
                    // 朝向目标
                    Quaternion rotation = Quaternion.LookRotation(lookDirection);
                    // 平滑转向目标
                    bug.transform.rotation = Quaternion.Slerp(bug.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
                }

                relative = launchPoint.position - transform.position;
                // 初始角度
                launchDirection = relative;
                // 无初始角度，正前方发射，无物理刚体
                //launchDirection = new Vector3(relative.x, 0, relative.z);
                if (isAiming)
                {
                    // 开始蓄力
                    if (Input.GetMouseButton(0))
                    {
                        isChargingShotEnabled = true;
                        if (isChargingShotEnabled)
                        {
                            chargeTime += Time.deltaTime;
                            // 渲染预测轨迹
                            DrawTrajectory(launchDirection, chargeTime);
                        }
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        chargeTime = 0;
                        isChargingShotEnabled = false;
                        CheckLineRenderer(follower);
                        fluidValue = bug.GetComponent<Player>().currentBodyFluid;
                        if (fluidValue >= fluidConsume)
                        {
                            // 记得角度和速度
                            bug.GetComponent<Player>().UseRangedAttack(fluidConsume);
                            StartCoroutine(ShootProjectile());
                        }
                        isShooting = false;
                    }
                }
            }
            else
            {
                isAiming = false;
                CheckLineRenderer(follower);
            }
        }
    }

    // 切换技能
    public void UpdateSkill(int ID)
    {
        // 对应预制体
        currentSkillID = ID;
    }

    void GetTargetPosition()
    {
        offset = bug.transform.position - mainCamera.transform.position;
        // 主相机发出射线，检测位置
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, offset, out hit))
        {
            // 物理检测位置为目标位置
            targetPosition = hit.point;
            Debug.DrawRay(ray.origin, offset, Color.yellow);
        }
    }

    void DrawTrajectory(Vector3 launchDirection, float chargeTime)
    {
        velocity = CalculateLaunchVelocity(launchDirection, chargeTime);
        Vector3 position = launchPoint.position;
        float timeStep = trajectoryPredicitionTimeStep;

        LineRenderer lineRenderer = follower.GetComponent<LineRenderer>();
        if (!lineRenderer)
        {
            lineRenderer = follower.AddComponent<LineRenderer>();
        }

        lineRenderer.startWidth = width;
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = maxTrajectoryPointCount;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float time = i * timeStep;
            Vector3 predictedPoint = CalculateTrajectoryPoint(position, velocity, Physics.gravity, time);
            Vector3 currentVelocity = CalculateCurrentVelocity(velocity, time);
            // 碰撞到地面停止预测
            RaycastHit hit;
            if (Physics.Raycast(predictedPoint, currentVelocity, out hit, 1f))
            {
                lineRenderer.positionCount = i;
                break;
            }

            lineRenderer.SetPosition(i, predictedPoint);
        }
    }

    // 检查是否留有轨迹
    void CheckLineRenderer(GameObject gameObject)
    {
        LineRenderer line = follower.GetComponent<LineRenderer>();
        if (line != null)
        {
            Destroy(line);
        }
    }

    // 计算初始速度矢量
    Vector3 CalculateLaunchVelocity(Vector3 launchDirection, float chargeTime)
    {
        float multiplier;
        if (chargeTime > maxChargeTime)
        {
            multiplier = 1;
        }
        else
        {
            multiplier = chargeTime / maxChargeTime;
        }
        return launchDirection * multiplier * forceMultiplier;
    }

    // 计算抛物线轨迹点位置
    Vector3 CalculateTrajectoryPoint(Vector3 position, Vector3 initialVelocity, Vector3 gravity, float time)
    {
        // 抛体
        return position + time * initialVelocity + 0.5f * gravity * Mathf.Pow(time, 2);
    }

    // 计算速度
    Vector3 CalculateCurrentVelocity(Vector3 velocity, float time)
    {
        return velocity + time * Physics.gravity;
    }
}
