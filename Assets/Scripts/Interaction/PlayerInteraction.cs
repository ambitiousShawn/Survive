using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject vehicle;
    private float distance = 5f;

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
            player.transform.position = vehicle.transform.position + vehicle.transform.right * distance;
        }
        else
        {
            // ȷ�����������
            vehicleController = true;
            vehicle.GetComponent<VehicleController>().ChangeState(true);
            player.GetComponent<PlayerController>().isMoving = false;
            lastUpdateTime = Time.time;
        }
    }
}
