using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GetDamaged : MonoBehaviour
{
	[Button]
	private void ReceiveDamage() {
		Orc[] orcs = FindObjectsOfType<Orc>();

		Orc target = orcs[Random.Range(0, orcs.Length)];


	}
}
