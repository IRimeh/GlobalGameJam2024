using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcSpawner : MonoBehaviour
{
	public GameObject OrcPrefab;

	public int InitalOrcs = 25;
	public int MaxOrcs = 80;

	public float InitialSpawnRate = 0.5f;
	public float SpawnRate = 10.0f;

	private float nextOrcTime = 0.0f;

	private List<Orc> Orcs = new List<Orc>();

	private void Update()
	{
		if(Time.time >= nextOrcTime)
		{
			if (Orcs.Count <= MaxOrcs)
			{
				if (InitalOrcs > 0)
				{
					InitalOrcs--;
					GameObject go = GameObject.Instantiate(OrcPrefab, transform.position, transform.rotation);
					go.GetComponent<Orc>().SetRandomWanderPos();
					Orcs.Add(go.GetComponent<Orc>());
					nextOrcTime = Time.time + InitialSpawnRate;
				}
				else
				{
					GameObject go = GameObject.Instantiate(OrcPrefab, transform.position, transform.rotation);
					go.GetComponent<Orc>().SetRandomWanderPos();
					Orcs.Add(go.GetComponent<Orc>());
					nextOrcTime = Time.time + SpawnRate;
				}
			}
		}
	}

	public void RemoveOrc(Orc pOrc) {
		Orcs.Remove(pOrc);
	}
}
