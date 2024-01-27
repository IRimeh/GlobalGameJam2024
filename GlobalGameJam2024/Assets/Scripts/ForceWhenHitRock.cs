using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceWhenHitRock : MonoBehaviour
{
    public float MinForce = 250.0f;
    public float MaxForce = 350.0f;
    public Rigidbody rigidBody;

    void Start()
    {
        Vector2 rand = Random.insideUnitCircle.normalized;
        Rock.OnRockHit += (Rock rock) => { rigidBody.AddForce(new Vector3(rand.x, 1.0f, rand.y) * Random.Range(MinForce, MaxForce), ForceMode.Impulse); };
    }
}
