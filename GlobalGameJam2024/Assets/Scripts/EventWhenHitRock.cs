using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventWhenHitRock : MonoBehaviour
{
    public UnityEvent rockHitEvent;

    void Awake()
    {
        Rock.OnRockHit += (Rock rock) => { rockHitEvent?.Invoke(); };
    }
}
