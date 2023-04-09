using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject player;
    public GameObject vehicle;
    private float distance = 5f;

    // 默认角色控制
    private bool playerController = true;
    private bool vehicleController = false;

    // 每秒恢复
    private float lastUpdateTime;

    public void Update()
    {
        if(vehicleController)
        {
            // 相机跟随
            player.transform.position = vehicle.transform.position;
            // 恢复
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
                    // 退出载具
                    playerController = true;
                    ChangeController();
                }
            }
            else
            {
                // 弹出按下F进入载具
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
            // 转换为角色控制
            vehicleController = false;
            vehicle.GetComponent<VehicleController>().ChangeState(false);
            player.GetComponent<PlayerController>().isMoving = true;
            player.transform.position = vehicle.transform.position + vehicle.transform.right * distance;
        }
        else
        {
            // 确保摄像机跟随
            vehicleController = true;
            vehicle.GetComponent<VehicleController>().ChangeState(true);
            player.GetComponent<PlayerController>().isMoving = false;
            lastUpdateTime = Time.time;
        }
    }
}
