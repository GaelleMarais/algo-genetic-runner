using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 10;

    void Update()
    {
        transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime; 
    }
}
