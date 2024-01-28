using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public Transform ShipTransform;
    public float BoatSpeed = 2.0f;
    public float BoatRotateSpeed = 0.5f;

    private enum ShipAttackDir
    {
        Left,
        Right
    }
    private ShipAttackDir shipAttackDir;
    public Transform targetTransform;


    private void OnEnable()
    {
        shipAttackDir = Random.Range(0, 1.0f) > 0.5f ? ShipAttackDir.Left : ShipAttackDir.Right;
        targetTransform.position = ShipTransform.position + ShipTransform.right * 40.0f * (shipAttackDir == ShipAttackDir.Left ? -1 : 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * BoatSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation, Time.deltaTime * BoatRotateSpeed);
    }
}
