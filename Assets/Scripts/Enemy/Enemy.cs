using Shawn.ProjectFramework;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;

    public string enemyName;      // ��������
    public GameObject enemyUIPrefab; // ���� UI Ԥ����

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ʵ�������� UI ������Ŀ�� transform

        // TODO���Ż����˽ṹ
        //enemyUI = Instantiate(enemyUIPrefab, FindObjectOfType<Canvas>().transform).GetComponent<EnemyUI>();
        //enemyUI.target = transform;

        //// ���õ������ƺͳ�ʼѪ��
        //enemyUI.SetNameAndHealth(enemyName, maxHealth);
    }

    // ����
    public void KnockBack(Vector3 direction, float time)
    {
        rb.AddForce(direction, ForceMode.Impulse);
    }
}
