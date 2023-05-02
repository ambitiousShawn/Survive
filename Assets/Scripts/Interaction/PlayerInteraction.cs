using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shawn.ProjectFramework;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent (typeof(BoxCollider))]
public class PlayerInteraction : MonoBehaviour
{
    private UGUI_MainUIPanel panel;

    private GameObject player;

    [SerializeField]
    [Header("�����")]
    private GameObject vehicle;

    [SerializeField]
    [Header("�����ݿɽ�������")]
    private float distance = 1.5f;

    [SerializeField]
    [Header("��Һ�ظ�ʱ����")]
    private float pickUpInterval = 1f;

    [SerializeField]
    [Header("�Ƿ�Ϊ��ҿ���")]
    private bool isPlayerController = true; 

    // ÿ��ָ�
    private float lastUpdateTime;

    private void Start()
    {
        player = gameObject;
    }

    public void Update()
    {
        if (panel == null) 
            panel = PanelManager.Instance.GetPanelByName("UGUI_MainUIPanel") as UGUI_MainUIPanel;
        
        //������ʻģʽ
        if (!isPlayerController)
        {
            // С�����
            player.transform.position = vehicle.transform.position;
            // �ָ�����
            if (Time.time - lastUpdateTime > pickUpInterval)
            {
                player.GetComponent<Player>().PickUp();
                lastUpdateTime = Time.time;
            }
            //�˳��ؾ�
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // �˳��ؾ�
                isPlayerController = true;
                ChangeController();
            }
        }
        //����ٿ�ģʽ
        else
        {
            Vector3 relative = player.transform.position - vehicle.transform.position;
            if (relative.sqrMagnitude <= Mathf.Pow(distance, 2))
            {
                panel?.ShowPickUp(vehicle.transform.position, "����ݣ�����E�������ڲ�������F������"); //��ʾ����
                // ��������F�����ؾ�
                if (Input.GetKeyDown(KeyCode.F))
                {
                    isPlayerController = false;
                    ChangeController();
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    // TODO:�����ڲ�
                }
            }
        }
    }
    

    public void ChangeController()
    {
        if (isPlayerController)
        {
            // ת��Ϊ��ɫ����
            //vehicle.GetComponent<VehicleController>().ChangeState(false);
            player.GetComponent<PlayerController>().isMoving = true;
            player.GetComponent<BoxCollider>().enabled = true;
            player.transform.position = vehicle.transform.position + Vector3.right * distance;
        }
        else
        {
            // ȷ�����������
            vehicle.GetComponent<VehicleController>().ChangeState(true);
            player.GetComponent<PlayerController>().isMoving = false;
            player.GetComponent<BoxCollider>().enabled = false;
            lastUpdateTime = Time.time;
        }
    }
}
