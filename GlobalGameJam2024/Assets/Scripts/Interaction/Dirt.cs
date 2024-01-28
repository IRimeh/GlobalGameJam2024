using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : AbstractInteractableObject
{
    private List<Orc> orcsCleaning = new List<Orc>();
    public bool isCleaned = false;

    public float TimeToClean = 10.0f;
    private float _timeCleaned = 0.0f;

    public GameObject ObjToDestroyWhenClean;

    public override bool IsWorkable(Orc orc)
    {
        return !isCleaned;
    }

    public override void OnStopTask(Orc orc)
    {
        if (orcsCleaning.Contains(orc))
            orcsCleaning.Remove(orc);
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

        orcsCleaning.Add(orc);

        while(!isCleaned)
        {
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
                orcsCleaning.RemoveAt(i);
            }
            Destroy(ObjToDestroyWhenClean);
        }
    }
}
