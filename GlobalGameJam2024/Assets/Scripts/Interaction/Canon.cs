using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Canon : AbstractInteractableObject
{
    float workProgress = 0;
    public int cannonballCount = 0;
    List<Orc> workerList = new List<Orc>();

    bool readyToFire = false;

    public int MaxOrcs = 3;
    public ParticleSystem OnFireParticleSystem;


    override public IEnumerator Task(Orc orc)
    {
        while (true)
        {
            orc.agent.destination = transform.position + (orc.transform.position - transform.position).normalized;
            workerList.Add(orc);

            float distance = (transform.position - orc.transform.position).magnitude;
            while (!readyToFire && cannonballCount > 0)
            {
                if (cannonballCount > 0 && distance < 2)//in working Range
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

            yield return new WaitForSeconds(0.1f);

            if(cannonballCount <= 0 && orc.isHoldingObj && orc.HoldingObj.TryGetComponent<Cannonball>(out _))
            {
                orc.animator.SetBool("isWorking", true);
                yield return new WaitForSeconds(2.0f);
                orc.animator.SetBool("isWorking", false);

                LoadCannonball();
                orc.StopHoldingObject(out GameObject holdingObj);
                Destroy(holdingObj.gameObject);
            }
        }
    }

    public void LoadCannonball()
    {
        cannonballCount = 1;
        workProgress = 0;
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
        if(!readyToFire && cannonballCount > 0)
        {
            workProgress += workerList.Count * 10 * Time.deltaTime;
            if(workProgress >= 100 ) 
            {
                readyToFire = true;
                cannonballCount--;
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
    }

    public override bool IsWorkable(Orc orc)
    {
        if (workerList.Count < MaxOrcs)
            return true;

        return false;
    }
}
