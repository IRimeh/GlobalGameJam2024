using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public ParticleSystem SplashParticles;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Orc"))
        {
            SplashParticles.transform.position = other.transform.position;
            SplashParticles.Play();
            Orc orc = null;
            Transform parent = other.transform.parent;
            while(orc == null)
            {
                parent.TryGetComponent<Orc>(out orc);
                parent = parent.parent;
            }
            Destroy(orc.gameObject);
        }
    }
}
