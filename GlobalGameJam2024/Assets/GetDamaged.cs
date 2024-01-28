using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.AI;

public class GetDamaged : MonoBehaviour
{
	public GameObject HolePrefab;

	[Button]
	private void ReceiveDamage() {
		Orc[] orcs = FindObjectsOfType<Orc>();

		Orc target = orcs[Random.Range(0, orcs.Length)];

		NavMeshHit hit;
		if (NavMesh.SamplePosition(target.transform.position, out hit, 1.0f, 1)) {
			GameObject hole = Instantiate(HolePrefab, hit.position, Quaternion.Euler(0.0f, Random.Range(0.0f, 359.9f), 0.0f));
			target.GetComponent<GetSplashed>().GetBonkedMethod();
		}
	}
}
