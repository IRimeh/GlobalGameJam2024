using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
	public float minRespawnTime = 30.0f;
	public float maxRespawnTime = 90.0f;

	private float nextSpawnTime = 0;

	public EnemyShip eShip;

	private void Start()
	{
		StartTime(eShip);
		EnemyShip.ShipSunk += StartTime;
	}

	private void StartTime(EnemyShip eShip) {
		nextSpawnTime = Time.time + Random.Range(minRespawnTime, maxRespawnTime);
	}

	public void Update()
	{
		if(Time.time >= nextSpawnTime)
		{
			eShip.Rise();
			nextSpawnTime = Mathf.Infinity;
		}

		if(Input.GetKeyDown(KeyCode.U))
		{
			eShip.Rise();
		}
	}
}
