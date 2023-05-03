using UnityEngine;
using Shawn.ProjectFramework;
using System.Runtime.CompilerServices;

public class CameraController : MonoBehaviour
{
    // �����ֵ�����������
    [SerializeField] private Transform cameraPivot;
    // ���λ��
    [SerializeField] private Transform player;
    // topλ��,���ڷ������߼�⣬��ײ�����޷����
    [SerializeField] private Transform top;
    // ��������
    [SerializeField] SphereCollider sphereCollider;

    // ��ʼ��Ծ���
    [SerializeField] private float maxOffset;
    // ��С���룬ȷ�����ᴩ������ģ��
    [SerializeField] private float minOffset;
    // ��¼�ڵ��仯
    private float offsetChange;
    // ����ƫ��
    [SerializeField] private float fixOffset;

    // ��������, ������ת
    private Vector3 relative;

    // �����Ǻ�ƫ����
    public float Pitch { get; private set; }
    public float Yaw { get; private set; }

    // ���������
    public float mouseSensitivity = 100f;
    // ƽ���̶�
    public float smoothSpeed = 5f;

    // ��ֱ�޷�
    [SerializeField] private float maxAngle;
    [SerializeField] private float minAngle;
    // ��ת�ٶ�
    [SerializeField] private float rotationSpeed = 1.0f;

    // ��Ұ����
    [SerializeField] private float maxFieldOfView;
    [SerializeField] private float minFieldOfView;
    [SerializeField] private float zoomSpeed;
    // ��ʼ��Ұ���ڵ�����
    private float originFieldOfView;

    // ��ס�ӽ�
    [SerializeField] private bool isLock = true;

    RaycastHit hit;

    private void Awake()
    {
        // �����ֵ����
        cameraPivot.rotation = player.rotation;
        // �������
        transform.rotation = cameraPivot.rotation;
    }

    private void Start()
    {
        sphereCollider = gameObject.GetComponent<SphereCollider>();
        originFieldOfView = maxFieldOfView;

        ReturnOrigin();
        // ����ʼ��
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

    // ���湦�ܣ������������λ��
    private void FollowPlayer()
    {
        // ���棬ʹ����ֵ��ֹ��ɫ��תӰ��
        // ͬʱ������ֵλ�����Ŀ��Լ򻯷��ڵ�������Ϸ���
        cameraPivot.position = top.position;
        // ���
        relative = transform.position - cameraPivot.position;
    }

    // �������
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
            // ����
            Cursor.lockState = CursorLockMode.Locked;
            // �������ɼ�
            Cursor.visible = false;
            isLock = true;
            player.GetComponent<PlayerController>().isMoving = true;
        }
    }

    // ���������ת
    private void FollowMouseRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * rotationSpeed;

        // ��ת����
        Yaw += mouseX;
        Pitch -= mouseY;

        // ����
        Pitch = Mathf.Clamp(Pitch, minAngle, maxAngle);

        // ��ת
        Quaternion newRotation = Quaternion.Euler(Pitch, Yaw, 0f);
        transform.rotation = newRotation;

        // λ��
        Vector3 newPositoin = cameraPivot.position - (newRotation * Vector3.forward) * offsetChange;
        transform.position = newPositoin;

        // ��ֵ����ˮƽ��ת
        Quaternion rotation = Quaternion.Euler(0f, Yaw, 0f);
        cameraPivot.rotation = rotation;
    }

    // ��������
    private void RollerScaling()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - zoom, minFieldOfView, maxFieldOfView);
        float ratio = Camera.main.fieldOfView / maxFieldOfView;
    }

    // ����ڵ�
    private void CheckBlock()
    {
        if (Physics.SphereCast(cameraPivot.position, sphereCollider.radius, relative, out hit, maxOffset, 3, QueryTriggerInteraction.Collide))
        {
            float targetOffset = Vector3.Dot(hit.point - cameraPivot.position, relative) - sphereCollider.radius - fixOffset;
            offsetChange = Mathf.Lerp(offsetChange, Mathf.Clamp(targetOffset, minOffset, maxOffset), smoothSpeed * Time.deltaTime);
        }
    }

    // ���ڵ���Ұ����
    private void AntiBlocking()
    {
        // ������ֵ��ȷ�����ֵ�Ĵ�С
        float ratio = offsetChange / maxOffset;
        maxFieldOfView = ratio * originFieldOfView;
    }

    // �ӽǻ���
    private void ReturnOrigin()
    {
        // ��ʼλ����ұ���
        transform.position = cameraPivot.position - maxOffset * player.forward;
        // �����Ұ��Χ
        Camera.main.fieldOfView = maxFieldOfView;
        // ��ʼ��
        relative = transform.position - cameraPivot.position;
        offsetChange = maxOffset;
    }

    // �������
    public void ShakeCamera()
    {

    }

    //-----------------------------------------------------
    // @author      OD
    // @version     1.0.0
    // @brief       �����ӽǼ�ʵ��
    //-----------------------------------------------------
    // ��ʼ�Ⱦ��ӽ�
    //[SerializeField] private Transform player;
    //[SerializeField] private Transform top;
    //// ƽ���̶�
    //public float smoothing = 5f;
    //// ƫ����
    //Vector3 offset;

    //// �ڵ�ƫ����
    //Vector3 offsetChange;

    //// ��������ʱ��
    //public float shakeDuration = 0.2f;
    //// ��������
    //public float shakeMangnitude = 0.5f;
    //// ��ʼλ��
    //private Vector3 originalPosition;
    //private float timeLeft = 0;

    //// ��¼�����С
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
    //    // �����,�����������
    //    transform.position = Vector3.Lerp(transform.position, player.position, smoothing * Time.deltaTime);

    //    // ���ڵ�,�����������仯
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
    //    // ���ڵ�,����Ϊ���
    //    if (Physics.Raycast(top.position, offset, out hit, offset.magnitude))
    //    {
    //        offsetChange = hit.point - player.position;
    //        Debug.DrawRay(top.position, offset, Color.yellow);
    //        return true;
    //    }
    //    return false;
    //}

    //// ��������ʱ��Ͷ������뼴��,������cinemachineҪ�õ�
    //public void ShakeCamera(float duration, float magnitude)
    //{
    //    originalPosition = transform.position;
    //    shakeDuration = duration;
    //    shakeMangnitude = magnitude;
    //    timeLeft = shakeDuration;
    //}
}
