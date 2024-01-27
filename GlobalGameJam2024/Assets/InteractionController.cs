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

                Orc orc = hit.collider.GetComponent<Orc>();
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (orc)
                {
                    if (!orcs.Contains(orc))
                    {
                        orcs.Add(orc);
                    }
                }
                else if (interactable)
                {
                    orcs[0]?.Work(interactable);
                    orcs[0]?.animator.SetBool("isWorking", true);
                }
                else
                {
                    orcs[0]?.agent.SetDestination(target);
                    orcs[0]?.animator.SetBool("isWorking", false);
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
