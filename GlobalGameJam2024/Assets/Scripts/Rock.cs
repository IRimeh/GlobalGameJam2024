using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [HideInInspector]
    public RockSpawner RockSpawner;

    public static Action<Rock> OnRockHit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ShipRockCollider")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/ShipCrash", transform.position);
            RockSpawner.ReplaceRock(this);
            OnRockHit?.Invoke(this);
        }
    }
}
