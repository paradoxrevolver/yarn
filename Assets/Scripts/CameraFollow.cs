using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 initialpos;
    private Vector3 updatepos;
    private Vector3 adjustpos;
    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");

        initialpos = player.transform.position;
    }
    private void LateUpdate()
    {
        updatepos = player.transform.position;
        updatepos = initialpos + updatepos;
        updatepos.z = updatepos.z - 20;
        updatepos.y = initialpos.y+20;
        transform.position = updatepos;
    }
}
