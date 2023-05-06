using Shawn.ProjectFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerController : MonoBehaviour
{
    [Header("�����Ϣ")]
    private CharacterMovement movement;
    [SerializeField] private Camera_3D pivot;
    [SerializeField] private Transform followTarget;


    [Header("����״̬��Ϣ")]
    // ����Ƿ�Ӵ�����
    [SerializeField] private bool isGrounded;
    // ��ɫ���ƣ������ؾ߽���
    public bool isMoving = true;
    // �����Ϣ
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
    /// ��ɫ�ƶ��İ�������ӿ�
    /// </summary>
    private void UpdateMovementInput()
    {
        Quaternion rot = Quaternion.Euler(0, pivot.Yaw, 0);
        movement.SetMovementInput(rot * Vector3.forward * Input.GetAxis("Vertical") + 
                                  rot * Vector3.right * Input.GetAxis("Horizontal"));
    }
}
