using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform follower;

    // Update is called once per frame
    void Update()
    {
        // 跟随发射点，线段渲染附着器
        transform.position = follower.position;
    }
}
