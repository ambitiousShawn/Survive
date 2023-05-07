using Shawn.ProjectFramework;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;

    public string enemyName;      // ��������
    public GameObject enemyUIPrefab; // ���� UI Ԥ����
    public EnemyUI enemyUI;
    // ����һ�� Canvas ����
    private Canvas enemyCanvas;

    // �������ֵ
    public float maxHealth = 100f;
    // ��ǰѪ��
    public float currentHealth;
    // ��Һֵ(����)
    public float maxBodyFluid = 100f;
    public float currentBodyFluid;

    private void Start()
    {
        // ���� Canvas ��Ϸ����
        GameObject canvasObject = Instantiate(enemyUIPrefab.gameObject, transform);
        // ��ȡ������Ϸ�����µ� Canvas ���
        enemyCanvas = canvasObject.GetComponent<Canvas>();

        enemyUI = canvasObject.GetComponent<EnemyUI>();

        enemyUI.InitialUI(enemyName, maxHealth, maxBodyFluid);
        // ��ʼ����ǰѪ��Ϊ���Ѫ��
        currentHealth = maxHealth;
        // ��ʼ����Һֵ
        currentBodyFluid = maxBodyFluid;
    }

    private void Update()
    {
        if (IsInCameraView())
        {
            // ���� UI ��ʾ
            enemyCanvas.enabled = true;
        }
        else
        {
            // ���� UI
            enemyCanvas.enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (IsInCameraView())
        {
            // ���� UI Ԫ�ص�λ��
            Vector3 pos = transform.position + transform.up;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);

            // �� UI Ԫ�ص�λ�ý�����Ļ����ת������������λ������
            enemyCanvas.transform.position = screenPos;

            // ʵ���� UI Ԫ�ز�������λ��
            RectTransform rectTransform = enemyCanvas.GetComponent<RectTransform>();
            rectTransform.position = screenPos;
        }
    }

    // TODO: �еĽ������жϣ�ͳһ��ʽ
    // �����ײ��ж�
    // �ܵ��˺��ķ����������˺����ս��ɸ÷�������
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            enemyUI.UpdateHealth(currentHealth);
        }
    }

    // ʹ����Һ����
    public void UseAttack(float usage)
    {
        currentBodyFluid -= usage;
        if (currentBodyFluid < 0)
        {
            currentBodyFluid = 0;
        }
        else
        {
            enemyUI.UpdateBodyFluid(currentBodyFluid);
        }
    }

    // ��Ѫ
    public void ContinueDamage(float damagePerSecond, float duration)
    {
        StartCoroutine(Continue());
        IEnumerator Continue()
        {
            float timer = 0f;
            while (timer < duration)
            {
                TakeDamage(damagePerSecond * Time.deltaTime);

                yield return null;

                timer += Time.deltaTime;
            }
        }
    }

    // ����
    public void Die()
    {
        Debug.Log("Enemy is dead");
        Destroy(gameObject);
        Destroy(enemyUI.gameObject);
    }

    // ����
    public void KnockBack(Vector3 direction)
    {
        rb.AddForce(direction, ForceMode.Impulse);
    }

    private bool IsInCameraView()
    {
        // �жϵ����Ƿ����������Ұ��
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        return screenPos.z > 0 && screenPos.x > 0 && screenPos.x < 1 && screenPos.y > 0 && screenPos.y < 1;
    }
}
