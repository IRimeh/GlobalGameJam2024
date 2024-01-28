using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RudderStagger : MonoBehaviour
{
	public Animator[] Rudders;

	public float minStagger = 0.00f;
	public float maxStagger = 0.03f;

	private void Start()
	{
		foreach (Animator a in Rudders) {
			a.SetFloat("Stagger", Random.Range(minStagger, maxStagger));
		}
	}
}
