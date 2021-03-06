﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset;
    public Transform player;
    public float smoothSpeed = 0.125f;
    

    void FixedUpdate() {
        offset = new Vector3(0, 11, -9);
        Vector3 desiredPos = player.position + offset;

        // If smooth scroll wanted, use this line of code, although player looks laggy
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = RoundVector(smoothedPos, 0.01f);
    }

    Vector3 RoundVector(Vector3 v, float roundTo) {
        return new Vector3(
            Mathf.Round(v.x / roundTo) * roundTo, 
            Mathf.Round(v.y / roundTo) * roundTo, 
            Mathf.Round(v.z / roundTo) * roundTo);
    }
}
