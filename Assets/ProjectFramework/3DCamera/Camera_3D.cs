using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_3D : MonoBehaviour
{
    public float Pitch { get; private set; }
    public float Yaw { get; private set; }

    public float mouseSensitivity = 5;
    public float cameraRotatingSpeed = 80;
    public float DelayYSpeed = 5;

    private Transform _target;
    private Transform _camera;

    [SerializeField] private AnimationCurve _animationCurve;

    private void Awake()
    {
        _camera = transform.GetChild(0);
    }

    public void InitCamera(Transform target)
    {
        _target = target;
        transform.position = _target.position;
    }

    void Update()
    {
        UpdateRotation();
        UpdatePosition();
        UpdateAnimationCurve();
    }

    private void UpdateRotation()
    {
        Yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        Yaw += Input.GetAxis("CameraRateX") * cameraRotatingSpeed * Time.deltaTime;
        Pitch += Input.GetAxis("Mouse Y") * mouseSensitivity;
        Pitch += Input.GetAxis("CameraRateY") * cameraRotatingSpeed * Time.deltaTime;
        Pitch = Mathf.Clamp(Pitch, -90, 90);

        transform.rotation = Quaternion.Euler(Pitch, Yaw, 0);
    }

    private void UpdatePosition()
    {
        float newY = Mathf.Lerp(transform.position.y, _target.position.y, Time.deltaTime * DelayYSpeed);
        transform.position = new Vector3(_target.position.x, newY, _target.position.z);
    }

    private void UpdateAnimationCurve()
    {
        _camera.localPosition = new Vector3(0, 0, -_animationCurve.Evaluate(Pitch));
    }
}
