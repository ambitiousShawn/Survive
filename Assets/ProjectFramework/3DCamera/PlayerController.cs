using Shawn.ProjectFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerController : MonoBehaviour
{
    [Header("组件信息")]
    private CharacterMovement movement;
    [SerializeField] private Camera_3D pivot;
    [SerializeField] private Transform followTarget;


    [Header("虫子状态信息")]
    // 检测是否接触地面
    [SerializeField] private bool isGrounded;
    // 角色控制，用于载具交互
    public bool isMoving = true;
    // 躲藏信息
    public bool isHide = false;

    private void Awake()
    {
        movement = GetComponent<CharacterMovement>();
        pivot.InitCamera(followTarget);
    }

    void Update()
    {
        UpdateMovementInput();        
    }

    /// <summary>
    /// 角色移动的按键输入接口
    /// </summary>
    private void UpdateMovementInput()
    {
        Quaternion rot = Quaternion.Euler(0, pivot.Yaw, 0);
        movement.SetMovementInput(rot * Vector3.forward * Input.GetAxis("Vertical") + 
                                  rot * Vector3.right * Input.GetAxis("Horizontal"));
    }
}
