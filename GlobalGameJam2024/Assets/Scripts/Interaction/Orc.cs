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
    public Rigidbody ragdollRigidBody;

    public Animator animator;

    public List<GameObject> ToDisableForRagdoll = new List<GameObject>();
    public CopyRagdoll CopyRagdoll;


    public bool isHoldingObj = false;
    public GameObject HoldingObj = null;

    public float minTimeToWander = 8.0f;
    public float maxTimeToWander = 15.0f;
    private float _wanderTimer = 0.0f;
    private float _neededWanderTime = 0.0f;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();    
        agent = GetComponent<NavMeshAgent>();
        _neededWanderTime = Random.Range(minTimeToWander, maxTimeToWander);
        _wanderTimer = Random.Range(0, _neededWanderTime);
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

        if(currentTask == null)
        {
            _wanderTimer += Time.deltaTime;
            if(_wanderTimer > _neededWanderTime)
            {
                // Wander to random spot
                Vector3 randomDirection = Random.insideUnitSphere * 5.0f;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, 5.0f, 1);
                Vector3 finalPosition = hit.position;
                agent.destination = finalPosition;

                _wanderTimer = 0.0f;
                _neededWanderTime = Random.Range(minTimeToWander, maxTimeToWander);
            }
        }
    }

    public void RagdollForSeconds(float seconds)
    {
        seconds += Random.Range(0.0f, 1.0f);
        StartCoroutine(UnRagdollTimer());
        IEnumerator UnRagdollTimer()
        {
            Vector3 dest = agent.destination;
            Ragdoll();
            yield return new WaitForSeconds(seconds);

            while (ragdollRigidBody.velocity.magnitude > .5f)
                yield return null;

            UnRagdoll();
            agent.destination = dest;
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


    public void HoldObject(GameObject gameObject)
    {
        isHoldingObj = true;
        gameObject.transform.SetParent(this.GetComponent<GrabPoit>().Hand);
        gameObject.transform.localPosition = Vector3.zero;
        HoldingObj = gameObject;
    }

    public void StopHoldingObject(out GameObject holdingObj)
    {
        holdingObj = HoldingObj;
        isHoldingObj = false;
        HoldingObj = null;
        gameObject.transform.SetParent(null);
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
