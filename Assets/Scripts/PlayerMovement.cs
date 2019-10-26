using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(0f, 0f, 1f * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(-1f * Time.deltaTime, 0f, 0f);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(0f, 0f, -1f * Time.deltaTime);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(1f * Time.deltaTime, 0f, 0f);
        }
    }
}
