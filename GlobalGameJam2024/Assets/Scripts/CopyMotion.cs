using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
	public Transform targetLimb;
	public bool mirror = true;

	Quaternion startRot;

	ConfigurableJoint joint;

	private void Start()
	{
		joint = GetComponent<ConfigurableJoint>();
		startRot = transform.localRotation;
	}

	private void Update()
	{
		if (!mirror) joint.targetRotation = targetLimb.localRotation * startRot;
		else joint.targetRotation = Quaternion.Inverse(targetLimb.localRotation) * startRot;
	}
}
