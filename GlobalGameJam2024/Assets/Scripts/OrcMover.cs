using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class OrcMover : MonoBehaviour
{
	public ConfigurableJoint joint;

	public Transform hips;
	public Transform rips;
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
			{
				GetComponent<NavMeshAgent>().destination = hit.point;
			}
		}

		joint.targetRotation = Quaternion.Euler(new Vector3(-transform.rotation.eulerAngles.y, 0.0f, 0.0f));

		rips.localPosition = Vector3.zero;
		hips.localPosition = Vector3.zero;
	}
}
