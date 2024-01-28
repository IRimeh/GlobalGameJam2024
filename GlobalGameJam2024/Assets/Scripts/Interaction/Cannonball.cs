using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public bool Shoot = false;
    public Rigidbody rigidBody;
    public float force = 100.0f;
    public LayerMask layerMask;
    public ParticleSystem explosionParticle;
    public MeshRenderer meshRenderer;
    public Collider coll;

    private bool hitShip = false;

    // Start is called before the first frame update
    void Start()
    {
        if(Shoot)
        {
            rigidBody.AddForce(transform.forward * force);
            DestroyAfterSeconds(10.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Shoot && !hitShip)
        {
            if ((layerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                hitShip = true;
                explosionParticle.Play();
                meshRenderer.enabled = false;
                coll.enabled = false;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/ShipGetShot", transform.position);
            }
        }
    }

    private IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
