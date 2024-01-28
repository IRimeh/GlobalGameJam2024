using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public Transform ShipTransform;
    public float BoatSpeed = 2.0f;
    public float BoatRotateSpeed = 0.5f;
    public int Health = 8;
    public Transform StartPos;
    public bool RiseOnAwake = true;
    public bool IsSunk = true;

    private int currentHealth = 8;

    private enum ShipAttackDir
    {
        Left,
        Right
    }
    private ShipAttackDir shipAttackDir;
    public Transform targetTransform;


    private void Awake()
    {
        if (RiseOnAwake) 
            Rise();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * BoatSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation, Time.deltaTime * BoatRotateSpeed);
    }

    public void TakeDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
            Sink();
    }


    [Button]
    public void Sink()
    {
        StartCoroutine(SinkCoroutine());
        IEnumerator SinkCoroutine()
        {
            for (float i = 0; i < 5; i += Time.deltaTime)
            {
                targetTransform.position += Vector3.down * 8.0f * Time.deltaTime;
                yield return null;
            }
            gameObject.SetActive(false);
            IsSunk = true;
        }
    }

    [Button]
    public void Rise()
    {
        StopAllCoroutines();
        IsSunk = false;
        gameObject.SetActive(true);
        transform.position = StartPos.position;
        transform.rotation = StartPos.rotation;

        currentHealth = Health;
        shipAttackDir = Random.Range(0, 1.0f) > 0.5f ? ShipAttackDir.Left : ShipAttackDir.Right;
        targetTransform.position = ShipTransform.position + ShipTransform.right * 40.0f * (shipAttackDir == ShipAttackDir.Left ? -1 : 1);
    }
}
