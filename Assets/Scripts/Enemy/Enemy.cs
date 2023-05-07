using Shawn.ProjectFramework;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;

    public string enemyName;      // 敌人名称
    public GameObject enemyUIPrefab; // 敌人 UI 预制体
    public EnemyUI enemyUI;
    // 定义一个 Canvas 变量
    private Canvas enemyCanvas;

    // 最大生命值
    public float maxHealth = 100f;
    // 当前血量
    public float currentHealth;
    // 体液值(方法)
    public float maxBodyFluid = 100f;
    public float currentBodyFluid;

    private void Start()
    {
        // 创建 Canvas 游戏对象
        GameObject canvasObject = Instantiate(enemyUIPrefab.gameObject, transform);
        // 获取敌人游戏对象下的 Canvas 组件
        enemyCanvas = canvasObject.GetComponent<Canvas>();

        enemyUI = canvasObject.GetComponent<EnemyUI>();

        enemyUI.InitialUI(enemyName, maxHealth, maxBodyFluid);
        // 初始化当前血量为最大血量
        currentHealth = maxHealth;
        // 初始化体液值
        currentBodyFluid = maxBodyFluid;
    }

    private void Update()
    {
        if (IsInCameraView())
        {
            // 更新 UI 显示
            enemyCanvas.enabled = true;
        }
        else
        {
            // 隐藏 UI
            enemyCanvas.enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (IsInCameraView())
        {
            // 计算 UI 元素的位置
            Vector3 pos = transform.position + transform.up;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);

            // 将 UI 元素的位置进行屏幕坐标转换，并更新其位置属性
            enemyCanvas.transform.position = screenPos;

            // 实例化 UI 元素并设置其位置
            RectTransform rectTransform = enemyCanvas.GetComponent<RectTransform>();
            rectTransform.position = screenPos;
        }
    }

    // TODO: 有的进行了判断，统一格式
    // 交给底层判断
    // 受到伤害的方法，所有伤害最终交由该方法处理
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

    // 使用体液攻击
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

    // 流血
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

    // 死亡
    public void Die()
    {
        Debug.Log("Enemy is dead");
        Destroy(gameObject);
        Destroy(enemyUI.gameObject);
    }

    // 击退
    public void KnockBack(Vector3 direction)
    {
        rb.AddForce(direction, ForceMode.Impulse);
    }

    private bool IsInCameraView()
    {
        // 判断敌人是否在主相机视野中
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        return screenPos.z > 0 && screenPos.x > 0 && screenPos.x < 1 && screenPos.y > 0 && screenPos.y < 1;
    }
}
