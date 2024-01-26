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
    Orc orc;

    private void Update()
    {
        
        if (Input.GetMouseButton(0))//Select orc
        {
            leftArmAnim.SetBool("isPointing", true);

            Vector3 target = transform.position;
            RaycastHit hit;
            Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);

            Physics.Raycast(ray, out hit);

            target = hit.point;

            
            if (hit.collider.GetComponent<Interactable>() != null) {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                orc.Work(interactable);
                orc.animator.SetBool("isWorking", true);
            }
            else
            {
                orc.agent.SetDestination(target);
                orc.animator.SetBool("isWorking", false);
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
