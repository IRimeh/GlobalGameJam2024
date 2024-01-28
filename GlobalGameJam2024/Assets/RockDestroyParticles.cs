using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDestroyParticles : MonoBehaviour
{
    public Transform PlayerRockParticlesTransform;
    public Transform EnemyRockParticlesTransform;
    public ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        Rock.OnRockHit += RockHit;
    }

    private void RockHit(Rock rock, bool isPlayer)
    {
        transform.position = isPlayer ? PlayerRockParticlesTransform.position : EnemyRockParticlesTransform.position + Vector3.up * 15.0f;
        particles.Play();
    }
}
