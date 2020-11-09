using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateWithCamera : MonoBehaviour
{
    public Camera cam;
    void Update()
    {
        transform.rotation = cam.transform.rotation;
    }
    
}
