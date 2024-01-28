using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipOnDirtListeneder : MonoBehaviour
{
	public SlipOnDirt sod;
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.GetComponent<Dirt>() != null)
		{
			sod.Slip();
		}
	}
}
