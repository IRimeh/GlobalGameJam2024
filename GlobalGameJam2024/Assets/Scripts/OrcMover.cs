using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class OrcMover : MonoBehaviour
{
	public ConfigurableJoint hipjoint;
	public Transform AnimationBase;

	private NavMeshAgent agent;

	private Vector3 HipPos;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		HipPos = hipjoint.transform.position;
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
			{
				agent.destination = hit.point;
			}
		}

		hipjoint.targetRotation = Quaternion.Euler(new Vector3(0.0f, -transform.rotation.eulerAngles.y, 0.0f));

		//hipjoint.transform.localPosition = HipPos;
	}
}
