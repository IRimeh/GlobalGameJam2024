using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Orc : MonoBehaviour
{
    public NavMeshAgent agent;

    public IEnumerator currentTask;
    public AbstractInteractableObject currentInteractable;

    public ConfigurableJoint hipJoint;
    public Rigidbody rigidBody;

    public Animator animator;

    public List<GameObject> ToDisableForRagdoll = new List<GameObject>();
    public GameObject RagdollObj;

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

    public void RagdollForSeconds(float seconds)
    {
        StartCoroutine(UnRagdollTimer());
        IEnumerator UnRagdollTimer()
        {
            Ragdoll();
            yield return new WaitForSeconds(seconds);
            UnRagdoll();
        }
    }

    public void Ragdoll()
    {
        agent.enabled = false;
        foreach (GameObject obj in ToDisableForRagdoll)
        {
            obj.SetActive(false);
        }
        RagdollObj.SetActive(true);
    }

    public void UnRagdoll()
    {
        agent.enabled = true;
        foreach (GameObject obj in ToDisableForRagdoll)
        {
            obj.SetActive(true);
        }
        RagdollObj.SetActive(false);
    }
}
