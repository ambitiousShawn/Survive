using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Transform target;    // ���˵� Transform
    public Image bgImage;       // ����ͼƬ
    public Text nameText;       // �����ı�
    public Slider healthSlider; // Ѫ��

    void LateUpdate()
    {
        // �� UI Ԫ��λ����Ϣת������Ļ����ϵ
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);

        // ���� UI Ԫ�ص�λ��
        bgImage.transform.position = screenPos;
        nameText.transform.position = screenPos + new Vector3(0, 20, 0);
        healthSlider.transform.position = screenPos + new Vector3(0, -10, 0);
    }

    // ���õ��˵����ƺͳ�ʼѪ��
    public void SetNameAndHealth(string name, float maxHealth)
    {
        nameText.text = name;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    // ���µ��˵�Ѫ��
    public void UpdateHealth(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
