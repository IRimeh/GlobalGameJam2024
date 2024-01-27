using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Orc : MonoBehaviour
{
    public NavMeshAgent agent;

    public Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();    
        agent = GetComponent<NavMeshAgent>();
    }

   public void Work(Interactable interactable)
    {
        agent.SetDestination(interactable.transform.position + (transform.position - interactable.transform.position).normalized);

        animator.SetBool("isWorking", true);
    }
}
