using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DropDirt : MonoBehaviour
{
	public float minDropTime = 60;
	public float maxDropTime = 240;

	public float engangeTime = 5;

	public GameObject DirtPrefab;

	private float nextDrop;

	private void Start()
	{
		nextDrop = Time.time + Random.Range(minDropTime, maxDropTime);
	}

	private void Update()
	{
		if (Time.time >= nextDrop) {
			nextDrop = Time.time + Random.Range(minDropTime,maxDropTime);
			if(UnityEngine.Random.Range(0, 1.0f) > 0.5f)
				Drop();
		}
	}

	private void Drop() {
		NavMeshHit hit;
		if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, 1)) {
			Quaternion qua = Quaternion.Euler(0.0f, Random.Range(0.0f, 359.0f), 0.0f);
			GameObject Dirt = GameObject.Instantiate(DirtPrefab, hit.position, qua);

			Dirt.GetComponentInChildren<Collider>().enabled = false;
			Dirt.transform.localScale = Vector3.zero;

			//Fluff can be removed
			Sequence mySec = DOTween.Sequence();
			mySec.Append(Dirt.transform.DOScaleX(1.0f, engangeTime));
			mySec.Join(Dirt.transform.DOScaleY(1.0f, engangeTime));
			mySec.Join(Dirt.transform.DOScaleZ(1.0f, engangeTime));
			mySec.AppendCallback(() =>EngangeDirt(Dirt));
			mySec.Play();
		}
	}

	private void EngangeDirt(GameObject pDirt) {
		pDirt.GetComponentInChildren<Collider>().enabled = true;
	}
}
