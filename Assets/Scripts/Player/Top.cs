using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Top : MonoBehaviour
{
    public Transform player;
    public Transform top;

    private Vector3 relative;

    private void Start()
    {
        relative = top.transform.position - player.transform.position;
    }
    void Update()
    {
        transform.position = player.position + relative;
    }
}
