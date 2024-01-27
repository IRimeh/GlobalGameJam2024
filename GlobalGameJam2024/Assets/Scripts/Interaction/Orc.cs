using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Orc : MonoBehaviour
{
    public NavMeshAgent agent;
    public ConfigurableJoint hipJoint;

    public IEnumerator currentTask;
    public AbstractInteractableObject currentInteractable;

    public Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();    
        agent = GetComponent<NavMeshAgent>();
    }

   public void Work(AbstractInteractableObject interactable)
    {
        StopAllCoroutines();
        currentInteractable = interactable;
        currentTask = currentInteractable.Task(this);
        StartCoroutine(currentTask);
    }

    public void StopTask()
    {
        StopAllCoroutines();
        if(currentInteractable != null)currentInteractable.OnStopTask(this);
        currentTask = null;
    }

    private void Update()
    {
        hipJoint.targetRotation = Quaternion.Euler(new Vector3(0.0f, -transform.rotation.eulerAngles.y, 0.0f));
    }
}
