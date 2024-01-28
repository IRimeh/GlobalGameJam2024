using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonkStick : MonoBehaviour
{
	public float cooldown = 1.0f;
	private float nextBonk = 0.0f;
	private bool isBonk = false;

	public void Bonk()
	{
		if (Time.time >= nextBonk && !isBonk)
		{
			nextBonk = Time.time + cooldown;
			isBonk = true;
		}
	}

		private void Update()
	{
		if (isBonk && Time.time >= nextBonk)
		{
			isBonk = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (isBonk && other.GetComponent<GetBonked>() != null) {
			other.GetComponent<GetBonked>().GetBonkedMethod();
		}
	}
}
