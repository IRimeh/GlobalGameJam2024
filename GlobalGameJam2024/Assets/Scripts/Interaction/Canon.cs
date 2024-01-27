using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : AbstractInteractableObject
{
    float workProgress = 0;
    int canonballCount = 3;
    List<Orc> workerList = new List<Orc>();

    bool readyToFire = false;


    override public IEnumerator Task(Orc orc)
    {
        orc.agent.destination = transform.position + (orc.transform.position - transform.position).normalized;
        while (true) 
        {
            float distance = (transform.position - orc.transform.position).magnitude;

            if (!readyToFire && canonballCount > 0 && distance < 2)//in working Range
            {
                orc.animator.SetBool("isWorking", true);
                workerList.Add(orc);
            }
            else
            {
                if(workerList.Contains(orc))workerList.Remove(orc);
                orc.animator.SetBool("isWorking", false);
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(1f);
        }  
    }

    override public void OnStopTask(Orc orc)
    {
        workerList.Remove(orc);
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
            if (Input.GetKeyDown(KeyCode.F))
            {
                Fire();
            }
        }
        
    }

    public void Fire()
    {
        readyToFire = false;

    }
}
