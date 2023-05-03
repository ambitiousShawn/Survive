using UnityEngine;
using Shawn.ProjectFramework;
using System.Runtime.CompilerServices;

public class CameraController : MonoBehaviour
{
    // 相机轴值，玩家子物体
    [SerializeField] private Transform cameraPivot;
    // 玩家位置
    [SerializeField] private Transform player;
    // top位置,用于发出射线检测，碰撞体内无法检测
    [SerializeField] private Transform top;
    // 球体检测器
    [SerializeField] SphereCollider sphereCollider;

    // 初始相对距离
    [SerializeField] private float maxOffset;
    // 最小距离，确保不会穿过人物模型
    [SerializeField] private float minOffset;
    // 记录遮挡变化
    private float offsetChange;
    // 修正偏移
    [SerializeField] private float fixOffset;

    // 方向向量, 保留旋转
    private Vector3 relative;

    // 俯仰角和偏航角
    public float Pitch { get; private set; }
    public float Yaw { get; private set; }

    // 鼠标灵敏度
    public float mouseSensitivity = 100f;
    // 平滑程度
    public float smoothSpeed = 5f;

    // 垂直限幅
    [SerializeField] private float maxAngle;
    [SerializeField] private float minAngle;
    // 旋转速度
    [SerializeField] private float rotationSpeed = 1.0f;

    // 视野限制
    [SerializeField] private float maxFieldOfView;
    [SerializeField] private float minFieldOfView;
    [SerializeField] private float zoomSpeed;
    // 初始视野，遮挡缩放
    private float originFieldOfView;

    // 锁住视角
    [SerializeField] private bool isLock = true;

    RaycastHit hit;

    private void Awake()
    {
        // 相机轴值正视
        cameraPivot.rotation = player.rotation;
        // 相机正视
        transform.rotation = cameraPivot.rotation;
    }

    private void Start()
    {
        sphereCollider = gameObject.GetComponent<SphereCollider>();
        originFieldOfView = maxFieldOfView;

        ReturnOrigin();
        // 光标初始化
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isLock = true;
    }

    private void FixedUpdate()
    {
        LockCursor();
        FollowPlayer();

        CheckBlock();
        AntiBlocking();
    }

    private void LateUpdate()
    {
        if (isLock)
        {
            FollowMouseRotation();
            RollerScaling();
        }
    }

    // 跟随功能，持续更新相对位置
    private void FollowPlayer()
    {
        // 跟随，使用轴值防止角色旋转影响
        // 同时，让轴值位于中心可以简化防遮挡，处理较方便
        cameraPivot.position = top.position;
        // 相对
        relative = transform.position - cameraPivot.position;
    }

    // 光标锁定
    private void LockCursor()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isLock = false;
            player.GetComponent<PlayerController>().isMoving = false;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            // 归中
            Cursor.lockState = CursorLockMode.Locked;
            // 锁定不可见
            Cursor.visible = false;
            isLock = true;
            player.GetComponent<PlayerController>().isMoving = true;
        }
    }

    // 跟随鼠标旋转
    private void FollowMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * rotationSpeed;

        // 旋转控制
        Yaw += mouseX;
        Pitch -= mouseY;

        // 限制
        Pitch = Mathf.Clamp(Pitch, minAngle, maxAngle);

        // 旋转
        Quaternion newRotation = Quaternion.Euler(Pitch, Yaw, 0f);
        transform.rotation = newRotation;

        // 位置
        Vector3 newPositoin = cameraPivot.position - (newRotation * Vector3.forward) * offsetChange;
        transform.position = newPositoin;

        // 轴值跟随水平旋转
        Quaternion rotation = Quaternion.Euler(0f, Yaw, 0f);
        cameraPivot.rotation = rotation;
    }

    // 滚轮缩放
    private void RollerScaling()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - zoom, minFieldOfView, maxFieldOfView);
        float ratio = Camera.main.fieldOfView / maxFieldOfView;
    }

    // 检查遮挡
    private void CheckBlock()
    {
        if (Physics.SphereCast(cameraPivot.position, sphereCollider.radius, relative, out hit, maxOffset, 3, QueryTriggerInteraction.Collide))
        {
            float targetOffset = Vector3.Dot(hit.point - cameraPivot.position, relative) - sphereCollider.radius - fixOffset;
            offsetChange = Mathf.Lerp(offsetChange, Mathf.Clamp(targetOffset, minOffset, maxOffset), smoothSpeed * Time.deltaTime);
        }
    }

    // 防遮挡视野缩放
    private void AntiBlocking()
    {
        // 根据轴值来确定最大值的大小
        float ratio = offsetChange / maxOffset;
        maxFieldOfView = ratio * originFieldOfView;
    }

    // 视角回旋
    private void ReturnOrigin()
    {
        // 初始位于玩家背后
        transform.position = cameraPivot.position - maxOffset * player.forward;
        // 相机视野范围
        Camera.main.fieldOfView = maxFieldOfView;
        // 初始化
        relative = transform.position - cameraPivot.position;
        offsetChange = maxOffset;
    }

    // 相机抖动
    public void ShakeCamera()
    {

    }

    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.0
    // @brief       正交视角简单实现
    //-----------------------------------------------------
    // 初始等距视角
    //[SerializeField] private Transform player;
    //[SerializeField] private Transform top;
    //// 平滑程度
    //public float smoothing = 5f;
    //// 偏移量
    //Vector3 offset;

    //// 遮挡偏移量
    //Vector3 offsetChange;

    //// 抖动持续时间
    //public float shakeDuration = 0.2f;
    //// 抖动距离
    //public float shakeMangnitude = 0.5f;
    //// 初始位置
    //private Vector3 originalPosition;
    //private float timeLeft = 0;

    //// 记录相机大小
    //private float size;
    //[SerializeField] private float minSize = 3f;
    //[SerializeField] private float minDistance = 1f;

    //void Start()
    //{
    //    PanelManager.Instance.ShowPanel<UGUI_MainUIPanel>("UGUI_MainUIPanel");
    //    offset = Camera.main.transform.position - player.position;
    //    size = Camera.main.orthographicSize;
    //}

    //private void FixedUpdate()
    //{
    //    // 轴跟随,带动相机跟随
    //    transform.position = Vector3.Lerp(transform.position, player.position, smoothing * Time.deltaTime);

    //    // 防遮挡,相机相对轴距离变化
    //    if (AntiBlocking())
    //    {
    //        if (offsetChange.magnitude < minDistance)
    //        {
    //            offsetChange = offsetChange.normalized * minDistance;
    //        }
    //        Vector3 changeCameraposition = player.position + offsetChange;
    //        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, changeCameraposition, smoothing * Time.deltaTime);
    //        float change = Camera.main.orthographicSize * offsetChange.magnitude / offset.magnitude;
    //        if (change < minSize)
    //        {
    //            change = minSize;
    //        }
    //        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, change, smoothing * Time.deltaTime);
    //    }
    //    else
    //    {
    //        Vector3 changeCameraposition = player.position + offset;
    //        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, changeCameraposition, smoothing * Time.deltaTime);
    //        float change = size;
    //        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, change, smoothing * Time.deltaTime);
    //    }

    //    originalPosition = transform.position;
    //    if (timeLeft > 0)
    //    {
    //        transform.position = originalPosition + Random.insideUnitSphere * shakeMangnitude;
    //        timeLeft -= Time.deltaTime;
    //    }
    //    else
    //    {
    //        timeLeft = 0;
    //        transform.position = originalPosition;
    //    }
    //}

    //private bool AntiBlocking()
    //{
    //    RaycastHit hit;
    //    // 有遮挡,距离为相对
    //    if (Physics.Raycast(top.position, offset, out hit, offset.magnitude))
    //    {
    //        offsetChange = hit.point - player.position;
    //        Debug.DrawRay(top.position, offset, Color.yellow);
    //        return true;
    //    }
    //    return false;
    //}

    //// 给出持续时间和抖动距离即可,具体用cinemachine要好点
    //public void ShakeCamera(float duration, float magnitude)
    //{
    //    originalPosition = transform.position;
    //    shakeDuration = duration;
    //    shakeMangnitude = magnitude;
    //    timeLeft = shakeDuration;
    //}
}
