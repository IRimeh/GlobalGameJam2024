using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dirt : AbstractInteractableObject
{
    private List<Orc> orcsCleaning = new List<Orc>();
    public bool isCleaned = false;

    public float TimeToClean = 10.0f;
    private float _timeCleaned = 0.0f;

    public GameObject ObjToDestroyWhenClean;
    public GameObject BroomPrefab;

    public override bool IsWorkable(Orc orc)
    {
        return !isCleaned;
    }

    public override void OnStopTask(Orc orc)
    {
        if (orc.isHoldingObj)
        {
            orc.StopHoldingObject(out GameObject holdObj);
            Destroy(holdObj);
        }

        if (orcsCleaning.Contains(orc))
            orcsCleaning.Remove(orc);

        List<Dirt> dirt = GameObject.FindObjectsOfType<Dirt>().ToList();
        dirt = dirt.OrderBy(x => UnityEngine.Random.value).ToList();
        Dirt dirtToClean = null;
        foreach (Dirt d in dirt)
        {
            if (!d.isCleaned)
            {
                dirtToClean = d;
                break;
            }
        }
        orc.Work(dirtToClean);
    }

    public override IEnumerator Task(Orc orc)
    {
        if (orc.isHoldingObj)
        {
            orc.StopHoldingObject(out GameObject holdObj);
            Destroy(holdObj);
        }
        GameObject broom = Instantiate(BroomPrefab);
        orc.HoldObject(broom);
        orc.agent.destination = transform.position + (orc.transform.position - transform.position).normalized;

        float distance = (transform.position - orc.transform.position).magnitude;
        while (distance > 2)
        {
            if (this == null)
                yield break;
            distance = (transform.position - orc.transform.position).magnitude;
            yield return new WaitForSeconds(0.1f);
        }

        orcsCleaning.Add(orc);

        while(!isCleaned)
        {
            if (this == null)
                yield break;
            yield return new WaitForSeconds(0.1f);
        }

        orc.StopTask();
    }

    private void Update()
    {
        _timeCleaned += Time.deltaTime * orcsCleaning.Count;
        if (_timeCleaned > TimeToClean)
        {
            isCleaned = true;

            for (int i = orcsCleaning.Count - 1; i >= 0; i--)
            {
                orcsCleaning[i].StopTask();
            }
            StopAllCoroutines();
            Destroy(ObjToDestroyWhenClean);
        }
    }
}
