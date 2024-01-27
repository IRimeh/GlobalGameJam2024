using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
	public Transform targetLimb;
	public bool mirror;

	ConfigurableJoint joint;

	private void Start()
	{
		joint = GetComponent<ConfigurableJoint>();
	}

	private void Update()
	{


		joint.targetRotation = mirror ? Quaternion.Inverse(targetLimb.localRotation) : targetLimb.localRotation;
	}
}
