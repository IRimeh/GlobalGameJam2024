using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<Canon> leftCannons = new List<Canon>();
    public List<Canon> rightCannons = new List<Canon>();
    public float ShootTime = 1;

    private bool isSinking = false;
    private int currentHealth = 8;
    private float shootTimer = 0;

    public static Action<EnemyShip> ShipSunk;


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

        if (!IsSunk && !isSinking)
        {
            ShootCannonBallsTimer();
        }
    }

    public void ShootCannonBallsTimer()
    {
        shootTimer += Time.deltaTime;

        if(shootTimer > ShootTime)
        {
            Shoot();
            shootTimer = 0;
        }
    }

    private void Shoot()
    {
        List<Canon> cannonsToShoot = shipAttackDir == ShipAttackDir.Left ? leftCannons : rightCannons;
        cannonsToShoot = cannonsToShoot.OrderBy(x => Random.value).ToList();
        cannonsToShoot[0].Fire();
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
            isSinking = true;
            for (float i = 0; i < 5; i += Time.deltaTime)
            {
                targetTransform.position += Vector3.down * 8.0f * Time.deltaTime;
                yield return null;
            }
            gameObject.SetActive(false);
            IsSunk = true;
            isSinking = false;
            ShipSunk?.Invoke(this);
        }
    }

    [Button]
    public void Rise()
    {
        StopAllCoroutines();
        IsSunk = false;
        isSinking = false;
        gameObject.SetActive(true);
        transform.position = StartPos.position;
        transform.rotation = StartPos.rotation;

        currentHealth = Health;
        shipAttackDir = Random.Range(0, 1.0f) > 0.5f ? ShipAttackDir.Left : ShipAttackDir.Right;
        targetTransform.position = ShipTransform.position + ShipTransform.right * 40.0f * (shipAttackDir == ShipAttackDir.Left ? 1 : -1);
    }
}
