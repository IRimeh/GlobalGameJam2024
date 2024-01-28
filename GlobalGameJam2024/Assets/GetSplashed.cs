using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSplashed : MonoBehaviour
{
	private Orc orc;

	public float splashForce = 600.0f;
	public float cooldown = 5.0f;
	private float nextbonk = 0.0f;

	private void Start()
	{
		orc = GetComponent<Orc>();
	}

	public void GetBonkedMethod()
	{
		if (Time.time >= nextbonk && !GetComponent<CopyRagdoll>().isRagdoll)
		{
			orc.RagdollForSeconds(5.0f);
			orc.ragdollRigidBody.AddForce((Vector3.up + new Vector3(Random.Range(-0.2f, 0.2f), 0.0f, Random.Range(-0.2f, 0.2f))).normalized * splashForce, ForceMode.Impulse);
			nextbonk = Time.time + cooldown;
		}
	}
}
