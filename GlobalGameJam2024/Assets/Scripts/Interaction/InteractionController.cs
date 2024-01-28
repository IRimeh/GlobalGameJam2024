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

            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit raycastHit in hits)
            {
                target = raycastHit.point;

                Orc hitOrc = raycastHit.collider.GetComponent<Orc>();
                AbstractInteractableObject interactable = raycastHit.collider.GetComponent<AbstractInteractableObject>();

                if (hitOrc)
                {
                    if (!orcs.Contains(hitOrc))
                    {
                        orcs.Add(hitOrc);
                    }
                }
                else if (interactable != null)
                {
                    if (orcs.Count > 0)
                    {
                        for (int i = orcs.Count - 1; i >= 0; i--)
                        {
                            if (interactable.IsWorkable(orcs[i]))
                            {
                                orcs[i].Work(interactable);
                                orcs.RemoveAt(i);
                            }
                        }
                        //orcs.Clear();
                    }

                }
                else
                {
                    if (orcs.Count > 0)
                    {
                        foreach (Orc orc in orcs)
                        {
                            if (orc.agent.enabled)
                            {
                                orc.agent.SetDestination(target);
                                orc.animator.SetBool("isWorking", false);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            leftArmAnim.SetBool("isPointing", false);
            orcs.Clear();
        }


        if (Input.GetMouseButtonDown(1))//Charge slap
        {
            
        }
        if (Input.GetMouseButtonUp(1)) //Release slap |give command
        {
            rightArmAnim.Play("Slap");
        }
    }

    private void OnDrawGizmos()
    {
        foreach (Orc orc in orcs)
        {
            Gizmos.color = new Color(0, 1.0f, 0, 0.6f);
            Gizmos.DrawSphere(orc.transform.position + Vector3.up * 1.0f, 0.25f);
        }
    }
}
