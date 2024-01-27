using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToOppositeRotation : MonoBehaviour
{
    public Transform OppositeOfWhatObject;

    void Update()
    {
        transform.rotation = Quaternion.Euler(-OppositeOfWhatObject.eulerAngles.x, -OppositeOfWhatObject.eulerAngles.y, -OppositeOfWhatObject.eulerAngles.z);
    }
}
