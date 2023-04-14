using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shawn.ProjectFramework;

public class PlayerInteraction : MonoBehaviour
{
    UGUI_MainUIPanel panel;

    public GameObject player;
    public GameObject vehicle;
    [SerializeField] private float distance = 1.5f;

    // Ĭ�Ͻ�ɫ����
    private bool playerController = true;
    private bool vehicleController = false;

    // ÿ��ָ�
    private float lastUpdateTime;

    public void Update()
    {
        if(vehicleController)
        {
            // �������
            player.transform.position = vehicle.transform.position;
            // �ָ�
            if (Time.time - lastUpdateTime > 1f)
            {
                player.GetComponent<Player>().PickUp();
                lastUpdateTime = Time.time;
            }
        }

        Vector3 relative = player.transform.position - vehicle.transform.position;
        if(relative.magnitude < distance)
        {
            panel?.ShowPickUp(vehicle.transform.position, "����ݣ�����E�������ڲ�������F������"); //��ʾ����
            if (vehicleController)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    // �˳��ؾ�
                    playerController = true;
                    ChangeController();
                }
            }
            else
            {
                // ��������F�����ؾ�
                if (Input.GetKeyDown(KeyCode.F))
                {
                    playerController = false;
                    ChangeController();
                }
                else if(Input.GetKeyDown(KeyCode.E))
                {
                    // �����ڲ�
                }
            }
        }
    }

    public void ChangeController()
    {
        if (playerController)
        {
            // ת��Ϊ��ɫ����
            vehicleController = false;
            vehicle.GetComponent<VehicleController>().ChangeState(false);
            player.GetComponent<PlayerController>().isMoving = true;
            player.GetComponent<BoxCollider>().enabled = true;
            player.transform.position = vehicle.transform.position + Vector3.right * distance;
        }
        else
        {
            // ȷ�����������
            vehicleController = true;
            vehicle.GetComponent<VehicleController>().ChangeState(true);
            player.GetComponent<PlayerController>().isMoving = false;
            player.GetComponent<BoxCollider>().enabled = false;
            lastUpdateTime = Time.time;
        }
    }
}
