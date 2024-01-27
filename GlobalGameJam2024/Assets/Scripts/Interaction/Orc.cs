using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
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
    public CopyRagdoll CopyRagdoll;

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
        if(currentInteractable != null) currentInteractable.OnStopTask(this);
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
        CopyRagdoll.EnableRagdoll();
    }

    public void UnRagdoll()
    {
        CopyRagdoll.EnableCharacter();
    }

    private void OnDrawGizmos()
    {
        if(currentTask != null)
        {
            Gizmos.color = new Color(0, 0.0f, 1.0f, 0.6f);
            Gizmos.DrawSphere(transform.position + Vector3.up * 1.0f, 0.3f);
        }
    }
}
