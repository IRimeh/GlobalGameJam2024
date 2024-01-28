using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Water : MonoBehaviour
{
    public ParticleSystem SplashParticles;

    public Vector3 SpawnPoint = new Vector3(12.0f, 10.0f, -3-0f);

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

            FindObjectOfType<OrcSpawner>().RemoveOrc(orc);

            Destroy(orc.gameObject);
        }

        if(other.gameObject.GetComponent<FirstPersonController>() != null) {
            other.gameObject.GetComponent<FirstPersonController>().Warp(SpawnPoint);
		}
    }
}
