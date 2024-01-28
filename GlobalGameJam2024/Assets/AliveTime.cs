using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveTime : MonoBehaviour
{
	public float StartTime = 0.0f;

	private void Start()
	{
		StartTime = Time.time;
	}

	public int GetAliveTime() {
		return Mathf.FloorToInt(Time.time - StartTime);
	}
}
