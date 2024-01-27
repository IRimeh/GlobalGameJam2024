using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
	//public Transform targetLimb;
	//public bool mirror = true;



	//ConfigurableJoint joint;

	//private void Start()
	//{
	//	joint = GetComponent<ConfigurableJoint>();
	//	startRot = transform.localRotation;
	//}

	//private void Update()
	//{
	//	if (!mirror) joint.targetRotation = targetLimb.localRotation * startRot;
	//	else joint.targetRotation = Quaternion.Inverse(targetLimb.localRotation) * startRot;
	//}

	public Transform AnimationBase;

	private List<ConfigurableJoint> Joints;
	private List<Transform> Partners = new List<Transform>();
	private List<Quaternion> StartRots = new List<Quaternion>();

	private void Start()
	{
		Joints = transform.GetComponentsInChildren<ConfigurableJoint>().ToList<ConfigurableJoint>();

		List<Transform> children = AnimationBase.GetComponentsInChildren<Transform>().ToList<Transform>();

		foreach (ConfigurableJoint joint in Joints) {
			Transform find = null;

			foreach (Transform c in children) {
				if (c.name == joint.name)
				{
					find = c;
				}
			}

			if (find == null)
			{
				Debug.Log("Couldnt find Partner.");
			}
			else { 
				Partners.Add(find);
			}
		}

		foreach(ConfigurableJoint joint in Joints)
		{
			StartRots.Add(joint.transform.localRotation);
		}

		Joints.RemoveAt(0);
		Partners.RemoveAt(0);
		StartRots.RemoveAt(0);
	}

	private void Update()
	{
		for(int i = 0; i < Joints.Count; i++)
		{
			Joints[i].targetRotation = Quaternion.Inverse(Partners[i].localRotation) * StartRots[i];
		}	
	}
}
