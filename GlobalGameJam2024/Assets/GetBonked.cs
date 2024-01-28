using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBonked : MonoBehaviour
{
	private Orc orc;

	public float bonkForce = 40.0f;
	public float cooldown = 5.0f;
	private float nextbonk = 0.0f;

	private void Start()
	{
		orc = GetComponent<Orc>();
	}

	public void GetBonkedMethod() { 
		if(Time.time >= nextbonk && !GetComponent<CopyRagdoll>().isRagdoll)
		{
			orc.RagdollForSeconds(3.0f);
			orc.ragdollRigidBody.AddForce(Camera.main.transform.right * bonkForce, ForceMode.Impulse);
			nextbonk = Time.time + cooldown;
		}
	}
}
