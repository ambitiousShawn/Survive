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
            panel?.ShowPickUp(vehicle.transform.position, "风滚草，按下E键进入内部，按下F键开启"); //显示交互
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
                else if(Input.GetKeyDown(KeyCode.E))
                {
                    // 加载内部
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
            player.GetComponent<BoxCollider>().enabled = true;
            player.transform.position = vehicle.transform.position + Vector3.right * distance;
        }
        else
        {
            // 确保摄像机跟随
            vehicleController = true;
            vehicle.GetComponent<VehicleController>().ChangeState(true);
            player.GetComponent<PlayerController>().isMoving = false;
            player.GetComponent<BoxCollider>().enabled = false;
            lastUpdateTime = Time.time;
        }
    }
}
