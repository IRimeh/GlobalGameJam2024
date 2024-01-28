using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventWhenHitRock : MonoBehaviour
{
    public UnityEvent rockHitEvent;
    public bool OnlyWhenPlayer = true;

    void Awake()
    {
        Rock.OnRockHit += (Rock rock, bool isPlayer) => { if(!OnlyWhenPlayer || isPlayer)rockHitEvent?.Invoke(); };
    }
}
