using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Transform target;    // 敌人的 Transform
    public Image bgImage;       // 背景图片
    public Text nameText;       // 名称文本
    public Slider healthSlider; // 血条

    void LateUpdate()
    {
        // 将 UI 元素位置信息转换到屏幕坐标系
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        // 设置 UI 元素的位置
        bgImage.transform.position = screenPos;
        nameText.transform.position = screenPos + new Vector3(0, 20, 0);
        healthSlider.transform.position = screenPos + new Vector3(0, -10, 0);
    }

    // 设置敌人的名称和初始血量
    public void SetNameAndHealth(string name, float maxHealth)
    {
        nameText.text = name;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    // 更新敌人的血量
    public void UpdateHealth(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
