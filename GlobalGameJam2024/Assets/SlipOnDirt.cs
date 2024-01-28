using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipOnDirt : MonoBehaviour
{
	public Rigidbody rb;

	public float cooldown = 10.0f;

	public float minForce = 20.0f;
	public float maxForce = 100.0f;

	private float nextSlip = 0.0f;

	public void Slip() {
		if (Time.time >= nextSlip)
		{
			nextSlip = Time.time + cooldown;
			GetComponent<Orc>().RagdollForSeconds(3.0f);
			rb.AddForce(transform.up * Random.Range(minForce, maxForce), ForceMode.Impulse);
			//rb.AddTorque(transform.right * -100000.0f, ForceMode.Impulse);
		}
	}
}
