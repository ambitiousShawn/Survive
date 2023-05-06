using Shawn.ProjectFramework;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;

    public string enemyName;      // 敌人名称
    public GameObject enemyUIPrefab; // 敌人 UI 预制体

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 实例化敌人 UI 并设置目标 transform

        // TODO：优化敌人结构
        //enemyUI = Instantiate(enemyUIPrefab, FindObjectOfType<Canvas>().transform).GetComponent<EnemyUI>();
        //enemyUI.target = transform;

        //// 设置敌人名称和初始血量
        //enemyUI.SetNameAndHealth(enemyName, maxHealth);
    }

    // 击退
    public void KnockBack(Vector3 direction, float time)
    {
        rb.AddForce(direction, ForceMode.Impulse);
    }
}
