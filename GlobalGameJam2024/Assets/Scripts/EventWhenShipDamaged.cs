using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventWhenShipDamaged : MonoBehaviour
{
    public UnityEvent shipDamageEvent;

    void Awake()
    {
        GetDamaged.OnShipDamaged += () => { shipDamageEvent?.Invoke(); };
    }
}
