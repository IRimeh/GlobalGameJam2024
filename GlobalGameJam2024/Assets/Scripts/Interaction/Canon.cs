using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Canon : AbstractInteractableObject
{
    float workProgress = 0;
    int canonballCount = 0;
    List<Orc> workerList = new List<Orc>();

    bool readyToFire = false;

    public int MaxOrcsFiring = 3;
    public int MaxOrcsLoading = 1;
    public ParticleSystem OnFireParticleSystem;

    private enum CannonState
    {
        Loading,
        Firing
    }
    private CannonState cannonState = CannonState.Loading;


    override public IEnumerator Task(Orc orc)
    {
        orc.agent.destination = transform.position + (orc.transform.position - transform.position).normalized;
        workerList.Add(orc);

        float distance = (transform.position - orc.transform.position).magnitude;
        switch (cannonState)
        {
            case CannonState.Loading:
                orc.agent.destination = transform.position + (orc.transform.position - transform.position).normalized;

                while (distance > 2)
                {
                    distance = (transform.position - orc.transform.position).magnitude;

                    yield return new WaitForSeconds(0.1f);
                }

                orc.animator.SetBool("isWorking", true);
                yield return new WaitForSeconds(3.0f);
                orc.animator.SetBool("isWorking", false);
                canonballCount = 1;
                cannonState = CannonState.Firing;
                workProgress = 0;
                orc.StopHoldingObject(out GameObject holdingObj);
                Destroy(holdingObj);
                break;
            case CannonState.Firing:

                while (!readyToFire && canonballCount > 0)
                {
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

                break;
        }

        ClearWorkerList();
    }

    private void ClearWorkerList()
    {
        for (int i = workerList.Count - 1; i >= 0; i--)
        {
            workerList[i].animator.SetBool("isWorking", false);
            workerList[i].StopTask();
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
        if(!readyToFire && cannonState == CannonState.Firing)
        {
            workProgress += workerList.Count * 10 * Time.deltaTime;
            if(workProgress >= 100 ) 
            {
                readyToFire = true;
                canonballCount--;
                workProgress = 0;
            }
        }

        if(readyToFire)
            Fire();
    }

    public void Fire()
    {
        readyToFire = false;

        OnFireParticleSystem.Play();
        transform.DOPunchScale(Vector3.one * 1.25f, 0.1f);
        workProgress = 0;

        if (canonballCount <= 0)
            cannonState = CannonState.Loading;
    }

    public override bool IsWorkable(Orc orc)
    {
        switch (cannonState)
        {
            case CannonState.Loading:
                if (orc.isHoldingObj && orc.HoldingObj.TryGetComponent<Cannonball>(out _) && workerList.Count < MaxOrcsLoading)
                    return true;
                break;
            case CannonState.Firing:
                if (canonballCount > 0 && workerList.Count < MaxOrcsFiring)
                    return true;
                break;
        }

        return false;
    }
}
