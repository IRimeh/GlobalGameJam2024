using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FrogAnimation : MonoBehaviour
{
	public NavMeshAgent Agent;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();

		animator.SetInteger("WalkType", Random.Range(1, 3));
	}

	private void Update()
	{
		animator.SetFloat("WalkSpeed", Agent.velocity.magnitude / Agent.speed);
	}
}
