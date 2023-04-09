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
    // ƽ���̶�
    public float smoothing = 5f;
    // ƫ����
    Vector3 offset;

    // �ڵ�ƫ����
    Vector3 offsetChange;

    // ��������ʱ��
    public float shakeDuration = 0.2f;
    // ��������
    public float shakeMangnitude = 0.5f;
    // ��ʼλ��
    private Vector3 originalPosition;
    private float timeLeft = 0;

    // ��¼�����С
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
        // �����,�����������
        transform.position = Vector3.Lerp(transform.position, player.position, smoothing * Time.deltaTime);

        // ���ڵ�,�����������仯
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
        // ���ڵ�,����Ϊ���
        if (Physics.Raycast(top.position, offset, out hit, offset.magnitude))
        {
            offsetChange = hit.point - player.position;
            Debug.DrawRay(top.position, offset, Color.yellow);
            return true;
        }
        return false;
    }

    // ��������ʱ��Ͷ������뼴��,������cinemachineҪ�õ�
    public void ShakeCamera(float duration, float magnitude)
    {
        originalPosition = transform.position;
        shakeDuration = duration;
        shakeMangnitude = magnitude;
        timeLeft = shakeDuration;
    }
}
