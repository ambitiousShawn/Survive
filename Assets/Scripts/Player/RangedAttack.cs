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

    // Ͷ����Ԥ����
    public GameObject[] projectilePerfab;
    // Ͷ���﷢���
    public Transform launchPoint;
    // ���䷽��
    private Vector3 launchDirection;
    // Ŀ��λ��
    private Vector3 targetPosition;
    // ������Player��ȷ������
    private Vector3 relative;

    // ָ���Զ�ſ�ת����ǿ������
    [SerializeField] private float distance = 1f;

    // �������
    private Vector3 offset;
    // ��Ⱦ���
    public float width = 1f;

    // ÿ���˺�
    public float damagePerSecond = 5f;
    // ����ʱ��
    public float slowDuration = 2f;

    private bool isAiming = false;
    private bool isShooting = false;

    // ��������ʱ����ȷ�������ٶ�
    [SerializeField] private float forceMultiplier = 10f;
    // �������ʱ��
    [SerializeField] private float maxChargeTime = 3f;
    // �켣Ԥ�ⲽ��
    [SerializeField] private float trajectoryPredicitionTimeStep = 0.1f;
    // �켣���������
    [SerializeField] private int maxTrajectoryPointCount = 100;
    // ��ǰ����ʱ��
    private float chargeTime = 0f;
    // ��ǰ�Ƿ�����
    private bool isChargingShotEnabled = false;
    // ��ǰ��Һֵ
    private float fluidValue;
    // ��¼Ԥ������ٶ�
    private Vector3 velocity;

    // �����л�
    [SerializeField] enum skill { first, second, third };

    private int currentSkillID = (int)skill.first;
    // �л����ݣ�ֻ��Ҫ�ж��ǵڼ����ܣ����ݼ��� ID ��ȷ���˺����ӵ���***��ʵ��***��
    // �����ؾ�״̬�ж���Ԥ�����л�

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

                // ת��
                Vector3 lookDirection = targetPosition - bug.transform.position;

                // ȷ��ֻ��ˮƽ������ת
                Vector3 look = new Vector3(lookDirection.x, 0, lookDirection.z);

                // ָ���Զʱ�ſ�����ת
                if (look.magnitude > distance)
                {
                    // ����Ŀ��
                    Quaternion rotation = Quaternion.LookRotation(lookDirection);
                    // ƽ��ת��Ŀ��
                    bug.transform.rotation = Quaternion.Slerp(bug.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
                }

                relative = launchPoint.position - transform.position;
                // ��ʼ�Ƕ�
                launchDirection = relative;
                // �޳�ʼ�Ƕȣ���ǰ�����䣬���������
                //launchDirection = new Vector3(relative.x, 0, relative.z);
                if (isAiming)
                {
                    // ��ʼ����
                    if (Input.GetMouseButton(0))
                    {
                        isChargingShotEnabled = true;
                        if (isChargingShotEnabled)
                        {
                            chargeTime += Time.deltaTime;
                            // ��ȾԤ��켣
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
                            // �ǵýǶȺ��ٶ�
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

    // �л�����
    public void UpdateSkill(int ID)
    {
        // ��ӦԤ����
        currentSkillID = ID;
    }

    void GetTargetPosition()
    {
        offset = bug.transform.position - mainCamera.transform.position;
        // ������������ߣ����λ��
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, offset, out hit))
        {
            // ������λ��ΪĿ��λ��
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
            // ��ײ������ֹͣԤ��
            RaycastHit hit;
            if (Physics.Raycast(predictedPoint, currentVelocity, out hit, 1f))
            {
                lineRenderer.positionCount = i;
                break;
            }

            lineRenderer.SetPosition(i, predictedPoint);
        }
    }

    // ����Ƿ����й켣
    void CheckLineRenderer(GameObject gameObject)
    {
        LineRenderer line = follower.GetComponent<LineRenderer>();
        if (line != null)
        {
            Destroy(line);
        }
    }

    // �����ʼ�ٶ�ʸ��
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

    // ���������߹켣��λ��
    Vector3 CalculateTrajectoryPoint(Vector3 position, Vector3 initialVelocity, Vector3 gravity, float time)
    {
        // ����
        return position + time * initialVelocity + 0.5f * gravity * Mathf.Pow(time, 2);
    }

    // �����ٶ�
    Vector3 CalculateCurrentVelocity(Vector3 velocity, float time)
    {
        return velocity + time * Physics.gravity;
    }
}
