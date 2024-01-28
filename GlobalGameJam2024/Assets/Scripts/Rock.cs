using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [HideInInspector]
    public RockSpawner RockSpawner;

    public static Action<Rock, bool> OnRockHit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ShipRockCollider")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/ShipCrash", transform.position + Vector3.up * 15.0f);
            Debug.Log(transform.position + Vector3.up * 15.0f);
            RockSpawner.ReplaceRock(this);
            OnRockHit?.Invoke(this, true);
            RockSpawner.ShipDamage.ReceiveDamage();
        }

        if (other.tag == "EnemyShipRockCollider")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/ShipCrash", transform.position + Vector3.up * 15.0f);
            RockSpawner.ReplaceRock(this);
            OnRockHit?.Invoke(this, false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + Vector3.up * 15.0f, 5.0f);
    }
}
