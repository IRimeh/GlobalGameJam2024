using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
	public float minRespawnTime = 30.0f;
	public float maxRespawnTime = 90.0f;

	private float nextSpawnTime = 0;
}
