using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Image bgImage;       // ����ͼƬ
    public Text nameText;       // �����ı�
    public Slider healthSlider; // Ѫ��
    public Slider bodyFluidSlider;  // ��Һ��

    // ���õ��˵����ƺͳ�ʼѪ��
    public void InitialUI(string name, float maxHealth, float maxBodyFluid)
    {
        nameText.text = name;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        bodyFluidSlider.maxValue = maxBodyFluid;
        bodyFluidSlider.value = maxBodyFluid;
    }

    // ���µ��˵�Ѫ��
    public void UpdateHealth(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }

    // ���µ�����Һֵ
    public void UpdateBodyFluid(float currentBodyFluid)
    {
        bodyFluidSlider.value = currentBodyFluid;
    }
}
