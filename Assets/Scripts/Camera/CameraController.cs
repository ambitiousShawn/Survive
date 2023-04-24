using UnityEngine;
using Shawn.ProjectFramework;

public class CameraController : MonoBehaviour
{
    // 相机轴值，玩家子物体
    [SerializeField] private Transform cameraPivot;
    // 玩家位置
    [SerializeField] private Transform player;
    // top位置,用于发出射线检测，碰撞体内无法检测
    [SerializeField] private Transform top;

    // 初始相对距离
    [SerializeField] private float maxOffset = 1f;
    // 最小距离，确保不会穿过人物模型
    [SerializeField] private float minOffset = 0f;
    // 记录遮挡变化
    private float offsetChange;
    // 记录遮挡变化量
    private float deltaChange;

    // 方向向量, 保留旋转
    private Vector3 relative;
    // 轴值和顶点间向量
    private Vector3 offset;

    // 俯仰角和偏航角
    public float Pitch { get; private set; }
    public float Yaw { get; private set; }

    // 鼠标灵敏度
    public float mouseSensitivity = 100f;
    // 平滑程度
    public float smoothSpeed = 5f;

    // 垂直限幅
    [SerializeField] private float maxAngle = 80f;
    [SerializeField] private float minAngle = -80f;
    // 旋转速度
    [SerializeField] private float rotationSpeed = 1.0f;

    // 视野限制
    [SerializeField] private float maxFieldOfView = 60f;
    [SerializeField] private float minFieldOfView = 20f;
    [SerializeField] private float zoomSpeed = 2f;

    // 锁住视角
    [SerializeField] private bool isLock = true;

    private void Awake()
    {
        // 相机轴值正视
        cameraPivot.rotation = player.rotation;
        // 相机正视
        transform.rotation = cameraPivot.rotation;
    }

    private void Start()
    {
        PanelManager.Instance.ShowPanel<UGUI_MainUIPanel>("UGUI_MainUIPanel");
        
        // 初始位于玩家背后
        transform.position = cameraPivot.position - maxOffset * player.forward;
        // 相机视野范围
        Camera.main.fieldOfView = maxFieldOfView;
        // 初始化
        relative = transform.position - cameraPivot.position;
        offset = cameraPivot.position - top.position;
        offsetChange = maxOffset;
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
        cameraPivot.position = player.position;
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
    }

    // 检查遮挡
    private void CheckBlock()
    {
        RaycastHit hit;
        float lastOffset = offsetChange;
        if (Physics.Raycast(top.position, relative + offset, out hit, maxOffset, 3))
        {
            // 获取距离，此时通过旋转跟随即可实现防遮挡
            offsetChange = Mathf.Clamp(hit.distance, minOffset, maxOffset);
            Debug.Log(offsetChange);
            Debug.DrawRay(top.position, relative + offset, Color.red);
        }
        else
        {
            offsetChange = maxOffset;
        }
        deltaChange = offsetChange - lastOffset;
    }

    // 防遮挡视野缩放
    private void AntiBlocking()
    {

    }

    // 视角回旋
    private void ReturnOrigin()
    {

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
