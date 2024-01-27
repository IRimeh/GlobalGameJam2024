using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : AbstractInteractableObject
{
    float workProgress = 0;
    int canonballCount = 1;
    List<Orc> workerList = new List<Orc>();

    bool readyToFire = false;

    public int MaxOrcs = 3;


    override public IEnumerator Task(Orc orc)
    {
        orc.agent.destination = transform.position + (orc.transform.position - transform.position).normalized;
        workerList.Add(orc);

        while (!readyToFire && canonballCount > 0)
        {
            float distance = (transform.position - orc.transform.position).magnitude;

            if (canonballCount > 0 && distance < 2)//in working Range
            {
                orc.animator.SetBool("isWorking", true);
            }
            else
            {
                orc.animator.SetBool("isWorking", false);
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(1f);
        }

        ClearWorkerList();
    }

    private void ClearWorkerList()
    {
        foreach (Orc orc in workerList)
        {
            orc.animator.SetBool("isWorking", false);
            orc.StopTask();
        }
        workerList.Clear();
    }

    override public void OnStopTask(Orc orc)
    {
        if(workerList.Contains(orc))
            workerList.Remove(orc);
        orc.animator.SetBool("isWorking", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!readyToFire)
        {
            workProgress += workerList.Count * 10 * Time.deltaTime;
            if(workProgress >= 100 ) 
            {
                readyToFire = true;
                canonballCount--;
                workProgress = 0;
            }
        }
        else
        {
            Fire();
        }
    }

    public void Fire()
    {
        readyToFire = false;
        Debug.Log("BOOOM");
    }

    public override bool IsWorkable()
    {
        bool workable = canonballCount > 0 && workerList.Count < MaxOrcs;
        return workable;
    }
}
