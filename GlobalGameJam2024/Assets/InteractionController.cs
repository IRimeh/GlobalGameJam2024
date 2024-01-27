using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField]
    Transform Camera;

    [SerializeField]
    Animator leftArmAnim, rightArmAnim;

    [SerializeField]
    List<Orc> orcs;

    private void Update()
    {
        
        if (Input.GetMouseButton(0))//Select orc
        {
            leftArmAnim.SetBool("isPointing", true);

            Vector3 target = transform.position;
            RaycastHit hit;
            Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);

            if(Physics.Raycast(ray, out hit))
            {
                target = hit.point;

                Orc hitOrc = hit.collider.GetComponent<Orc>();
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (hitOrc)
                {
                    if (!orcs.Contains(hitOrc))
                    {
                        orcs.Add(hitOrc);
                    }
                }
                else if (interactable)
                {
                    if(orcs.Count > 0) {
                        foreach(Orc orc in orcs) {
                            orc.Work(interactable);
                            orc.animator.SetBool("isWorking", true);
                        }
                        orcs.Clear();
                    }
                    
                }
                else
                {
                    if (orcs.Count > 0)
                    {
                        foreach (Orc orc in orcs)
                        {
                            orc.agent.SetDestination(target);
                            orc.animator.SetBool("isWorking", false);
                        }
                    }
                }
            } 
        }
        else
        {
            leftArmAnim.SetBool("isPointing", false);
        }
        if (Input.GetMouseButtonDown(1))//Charge slap
        {
            
        }
        if (Input.GetMouseButtonUp(1)) //Release slap |give command
        {
            rightArmAnim.Play("Slap");
        }
    }
}
