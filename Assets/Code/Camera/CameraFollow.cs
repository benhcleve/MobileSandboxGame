using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTransform;

    void LateUpdate()
    {
        transform.position = followTransform.position;
    }
}
