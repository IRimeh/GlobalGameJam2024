using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (Camera.main == null)
            return;

        transform.rotation = Camera.main.transform.rotation * originalRotation;
    }
}