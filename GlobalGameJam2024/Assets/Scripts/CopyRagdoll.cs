using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CopyRagdoll : MonoBehaviour
{
	public Transform RagdollBase;
	public Transform ModelBase;

	public Transform HipAnchor;

	public float MaxGetUpDistance = 3.0f;

	private List<ConfigurableJoint> Joints;
	private List<Transform> Partners = new List<Transform>();

	private List<Rigidbody> Rigidbodies;
	private Collider[] Collideres;
	private SkinnedMeshRenderer ModelRenderer;

	private NavMeshAgent Agent;

	private void Start()
	{
		Agent = GetComponent<NavMeshAgent>();
		Rigidbodies = ModelBase.GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>();
		for (int i = 0; i < Rigidbodies.Count; i++) { if (Rigidbodies[i].transform == HipAnchor) { Rigidbodies.RemoveAt(i); } }
		Collideres = ModelBase.GetComponentsInChildren<Collider>();
		ModelRenderer = ModelBase.GetComponentsInChildren<SkinnedMeshRenderer>()[0];

		Joints = transform.GetComponentsInChildren<ConfigurableJoint>().ToList<ConfigurableJoint>();

		List<Transform> children = RagdollBase.GetComponentsInChildren<Transform>().ToList<Transform>();

		foreach (ConfigurableJoint joint in Joints)
		{
			Transform find = null;

			foreach (Transform c in children)
			{
				if (c.name == joint.name)
				{
					find = c;
				}
			}

			if (find == null)
			{
				Debug.Log("Couldnt find Partner.");
			}
			else
			{
				Partners.Add(find);
			}
		}
	}

	[Button]
	public void EnableRagdoll() {
		for (int i = 0; i < Partners.Count; i++) { 
			Partners[i].SetLocalPositionAndRotation(Joints[i].transform.localPosition, Joints[i].transform.localRotation);
		}

		foreach (Rigidbody r in Rigidbodies)
		{
			r.isKinematic = true;
			r.useGravity = false;
		}

		foreach (Collider c in Collideres)
		{
			c.enabled = false;
		}

		ModelRenderer.enabled = false;

		RagdollBase.gameObject.SetActive(true);

		Agent.enabled = false;
	}

	[Button]
	public bool EnableCharacter()
	{
		for (int i = 0; i < Partners.Count; i++)
		{
			//Partners[i].SetLocalPositionAndRotation(Joints[i].transform.localPosition, Joints[i].transform.localRotation);
			Joints[i].transform.SetLocalPositionAndRotation(Partners[i].localPosition, Partners[i].localRotation);
		}

		NavMeshHit hit;

		if (NavMesh.SamplePosition(Joints[0].transform.position, out hit, MaxGetUpDistance, 1)) {
			List<Transform> children = new List<Transform>();

			foreach (Transform t in transform)
			{
				if (t != transform)
				{
					children.Add(t);
					t.parent = null;
				}
			}

			transform.position = hit.position;

			foreach (Transform child in children) { child.parent = transform; }

			ModelBase.gameObject.SetActive(true);

			foreach (Rigidbody r in Rigidbodies)
			{
				r.isKinematic = false;
				r.useGravity = true;
			}

			foreach (Collider c in Collideres)
			{
				c.enabled = true;
			}

			ModelRenderer.enabled = true;

			RagdollBase.gameObject.SetActive(false);

			Agent.enabled = true;

			return true;
		}

		return false;
	}
}
