using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonballPile : AbstractInteractableObject
{
    public GameObject CannonballPrefab;
    public List<Canon> Cannons = new List<Canon>();

    public override bool IsWorkable(Orc orc)
    {
        return !orc.isHoldingObj;
    }

    public override void OnStopTask(Orc orc)
    {
        
    }

    public override IEnumerator Task(Orc orc)
    {
        orc.agent.destination = transform.position + (orc.transform.position - transform.position).normalized;

        float distance = (transform.position - orc.transform.position).magnitude;
        while (distance > 2)
        {
            distance = (transform.position - orc.transform.position).magnitude;

            yield return new WaitForSeconds(0.1f);
        }

        GameObject cannonBall = Instantiate(CannonballPrefab);
        orc.HoldObject(cannonBall);
        orc.StopTask();
    }
}
