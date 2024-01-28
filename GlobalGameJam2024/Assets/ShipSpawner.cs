using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
	public float minRespawnTime = 30.0f;
	public float maxRespawnTime = 90.0f;

	private float nextSpawnTime = 0;

	private EnemyShip eShip;

	private void Start()
	{
		eShip = GetComponent<EnemyShip>();
		nextSpawnTime = Time.time + Random.Range(minRespawnTime, maxRespawnTime);
	}

	public void Update()
	{
		
	}
}
