using UnityEngine;
using Shawn.ProjectFramework;

public class CameraController : MonoBehaviour
{
    // �����ֵ�����������
    [SerializeField] private Transform cameraPivot;
    // ���λ��
    [SerializeField] private Transform player;
    // topλ��,���ڷ������߼�⣬��ײ�����޷����
    [SerializeField] private Transform top;

    // ��ʼ��Ծ���
    [SerializeField] private float maxOffset = 1f;
    // ��С���룬ȷ�����ᴩ������ģ��
    [SerializeField] private float minOffset = 0f;
    // ��¼�ڵ��仯
    private float offsetChange;
    // ��¼�ڵ��仯��
    private float deltaChange;

    // ��������, ������ת
    private Vector3 relative;
    // ��ֵ�Ͷ��������
    private Vector3 offset;

    // �����Ǻ�ƫ����
    public float Pitch { get; private set; }
    public float Yaw { get; private set; }

    // ���������
    public float mouseSensitivity = 100f;
    // ƽ���̶�
    public float smoothSpeed = 5f;

    // ��ֱ�޷�
    [SerializeField] private float maxAngle = 80f;
    [SerializeField] private float minAngle = -80f;
    // ��ת�ٶ�
    [SerializeField] private float rotationSpeed = 1.0f;

    // ��Ұ����
    [SerializeField] private float maxFieldOfView = 60f;
    [SerializeField] private float minFieldOfView = 20f;
    [SerializeField] private float zoomSpeed = 2f;

    // ��ס�ӽ�
    [SerializeField] private bool isLock = true;

    private void Awake()
    {
        // �����ֵ����
        cameraPivot.rotation = player.rotation;
        // �������
        transform.rotation = cameraPivot.rotation;
    }

    private void Start()
    {
        PanelManager.Instance.ShowPanel<UGUI_MainUIPanel>("UGUI_MainUIPanel");
        
        // ��ʼλ����ұ���
        transform.position = cameraPivot.position - maxOffset * player.forward;
        // �����Ұ��Χ
        Camera.main.fieldOfView = maxFieldOfView;
        // ��ʼ��
        relative = transform.position - cameraPivot.position;
        offset = cameraPivot.position - top.position;
        offsetChange = maxOffset;
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
        cameraPivot.position = player.position;
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
    }

    // ����ڵ�
    private void CheckBlock()
    {
        RaycastHit hit;
        float lastOffset = offsetChange;
        if (Physics.Raycast(top.position, relative + offset, out hit, maxOffset, 3))
        {
            // ��ȡ���룬��ʱͨ����ת���漴��ʵ�ַ��ڵ�
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

    // ���ڵ���Ұ����
    private void AntiBlocking()
    {

    }

    // �ӽǻ���
    private void ReturnOrigin()
    {

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
