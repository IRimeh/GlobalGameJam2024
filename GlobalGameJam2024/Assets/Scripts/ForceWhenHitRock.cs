using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceWhenHitRock : MonoBehaviour
{
    public float MinForce = 250.0f;
    public float MaxForce = 350.0f;
    public Rigidbody rigidBody;
    public bool OnlyWhenPlayer = true;

    void Start()
    {
        Rock.OnRockHit += ApplyForce;
    }

    public void ApplyForce(Rock rock, bool isPlayer)
    {
        Vector2 rand = Random.insideUnitCircle.normalized;
        if (!OnlyWhenPlayer || isPlayer)
            rigidBody.AddForce(new Vector3(rand.x, 1.0f, rand.y) * Random.Range(MinForce, MaxForce), ForceMode.Impulse);
    }

    private void OnDestroy()
    {
        Rock.OnRockHit -= ApplyForce;
    }
}
