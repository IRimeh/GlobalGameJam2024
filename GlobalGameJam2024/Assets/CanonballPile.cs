using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        while (true)
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

            yield return LoadCannon(orc);
        }
    }

    private IEnumerator LoadCannon(Orc orc)
    {
        FindCannon:
        Canon cannonToLoad = null;
        while (cannonToLoad == null)
        {
            Cannons = Cannons.OrderBy(x => Random.value).ToList();
            foreach (var cannon in Cannons)
            {
                if (cannon.cannonballCount <= 0)
                {
                    cannonToLoad = cannon;
                    break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }

        orc.agent.destination = cannonToLoad.transform.position + (orc.transform.position - transform.position).normalized;
        float distance = (cannonToLoad.transform.position - orc.transform.position).magnitude;
        while (distance > 2)
        {
            distance = (cannonToLoad.transform.position - orc.transform.position).magnitude;

            yield return new WaitForSeconds(0.1f);
        }
        if (cannonToLoad.cannonballCount > 0)
            goto FindCannon;
        orc.animator.SetBool("isWorking", true);
        yield return new WaitForSeconds(3.0f);
        orc.animator.SetBool("isWorking", false);
        if (cannonToLoad.cannonballCount > 0)
            goto FindCannon;

        cannonToLoad.cannonballCount = 1;
        orc.StopHoldingObject(out GameObject holdingObj);
        Destroy(holdingObj.gameObject);
    }
}
