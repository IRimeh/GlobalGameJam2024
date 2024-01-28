using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSplashTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.GetComponent<Hole>() != null)
		{
			GetComponent<GetSplashed>().GetBonkedMethod();

			other.GetComponent<Hole>().Splash.Play();
		}
	}
}
