using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform follower;

    // Update is called once per frame
    void Update()
    {
        // ���淢��㣬�߶���Ⱦ������
        transform.position = follower.position;
    }
}
