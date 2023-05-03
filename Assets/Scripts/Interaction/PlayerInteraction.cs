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
    [Header("风滚草")]
    private GameObject vehicle;

    [SerializeField]
    [Header("与风滚草可交互距离")]
    private float distance = 1.5f;

    [SerializeField]
    [Header("体液回复时间间隔")]
    private float pickUpInterval = 1f;

    [SerializeField]
    [Header("是否为玩家控制")]
    private bool isPlayerController = true; 

    // 每秒恢复
    private float lastUpdateTime;

    private void Start()
    {
        player = gameObject;
    }

    public void Update()
    {
        if (panel == null) 
            panel = PanelManager.Instance.GetPanelByName("UGUI_MainUIPanel") as UGUI_MainUIPanel;
        
        //车辆驾驶模式
        if (!isPlayerController)
        {
            // 小虫跟随
            player.transform.position = vehicle.transform.position;
            // 恢复属性
            if (Time.time - lastUpdateTime > pickUpInterval)
            {
                player.GetComponent<Player>().PickUp();
                lastUpdateTime = Time.time;
            }
            //退出载具
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // 退出载具
                isPlayerController = true;
                ChangeController();
            }
        }
        //人物操控模式
        else
        {
            Vector3 relative = player.transform.position - vehicle.transform.position;
            if (relative.sqrMagnitude <= Mathf.Pow(distance, 2))
            {
                panel?.ShowPickUp(vehicle.transform.position, "风滚草，按下E键进入内部，按下F键开启"); //显示交互
                // 弹出按下F进入载具
                if (Input.GetKeyDown(KeyCode.F))
                {
                    isPlayerController = false;
                    ChangeController();
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    // TODO:加载内部
                }
            }
        }
    }
    

    public void ChangeController()
    {
        if (isPlayerController)
        {
            // 转换为角色控制
            //vehicle.GetComponent<VehicleController>().ChangeState(false);
            player.GetComponent<PlayerController>().isMoving = true;
            player.GetComponent<BoxCollider>().enabled = true;
            player.transform.position = vehicle.transform.position + Vector3.right * distance;
        }
        else
        {
            // 确保摄像机跟随
            vehicle.GetComponent<VehicleController>().ChangeState(true);
            player.GetComponent<PlayerController>().isMoving = false;
            player.GetComponent<BoxCollider>().enabled = false;
            lastUpdateTime = Time.time;
        }
    }
}
