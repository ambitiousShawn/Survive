using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Image bgImage;       // 背景图片
    public Text nameText;       // 名称文本
    public Slider healthSlider; // 血条
    public Slider bodyFluidSlider;  // 体液条

    // 设置敌人的名称和初始血量
    public void InitialUI(string name, float maxHealth, float maxBodyFluid)
    {
        nameText.text = name;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        bodyFluidSlider.maxValue = maxBodyFluid;
        bodyFluidSlider.value = maxBodyFluid;
    }

    // 更新敌人的血量
    public void UpdateHealth(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }

    // 更新敌人体液值
    public void UpdateBodyFluid(float currentBodyFluid)
    {
        bodyFluidSlider.value = currentBodyFluid;
    }
}
