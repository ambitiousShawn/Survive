using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Shawn.ProjectFramework;
using Unity.VisualScripting;
//using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform top;
    // 平滑程度
    public float smoothing = 5f;
    // 偏移量
    Vector3 offset;

    // 遮挡偏移量
    Vector3 offsetChange;

    // 抖动持续时间
    public float shakeDuration = 0.2f;
    // 抖动距离
    public float shakeMangnitude = 0.5f;
    // 初始位置
    private Vector3 originalPosition;
    private float timeLeft = 0;

    // 记录相机大小
    private float size;
    [SerializeField] private float minSize = 3f;

    void Start()
    {
        PanelManager.Instance.ShowPanel<UGUI_MainUIPanel>("UGUI_MainUIPanel");
        offset = Camera.main.transform.position - player.position;
        size = Camera.main.orthographicSize;
    }

    private void FixedUpdate()
    {
        // 轴跟随,带动相机跟随
        transform.position = Vector3.Lerp(transform.position, player.position, smoothing * Time.deltaTime);

        // 防遮挡,相机相对轴距离变化
        if (AntiBlocking())
        {
            Vector3 changeCameraposition = player.position + offsetChange;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, changeCameraposition, smoothing * Time.deltaTime);
            float change = Camera.main.orthographicSize * offsetChange.magnitude / offset.magnitude;
            if (change < minSize)
            {
                change = minSize;
            }
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, change, smoothing * Time.deltaTime);
        }
        else
        {
            Vector3 changeCameraposition = player.position + offset;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, changeCameraposition, smoothing * Time.deltaTime);
            float change = size;
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, change, smoothing * Time.deltaTime);
        }

        originalPosition = transform.position;
        if (timeLeft > 0)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeMangnitude;
            timeLeft -= Time.deltaTime;
        }
        else
        {
            timeLeft = 0;
            transform.position = originalPosition;
        }
    }

    private bool AntiBlocking()
    {
        RaycastHit hit;
        // 有遮挡,距离为相对
        if (Physics.Raycast(top.position, offset, out hit, offset.magnitude))
        {
            offsetChange = hit.point - player.position;
            Debug.DrawRay(top.position, offset, Color.yellow);
            return true;
        }
        return false;
    }

    // 给出持续时间和抖动距离即可,具体用cinemachine要好点
    public void ShakeCamera(float duration, float magnitude)
    {
        originalPosition = transform.position;
        shakeDuration = duration;
        shakeMangnitude = magnitude;
        timeLeft = shakeDuration;
    }
}
