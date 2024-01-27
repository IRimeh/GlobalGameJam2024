using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public LayerMask mask;
    public Canvas UI;

    void Update()
    {
        bool hitInteractable = false;
        PlayerInteractable interactable = null;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2.0f, mask))
        {
            if(hit.collider.TryGetComponent<PlayerInteractable>(out interactable))
                hitInteractable = true;
        }

        UI.gameObject.SetActive(hitInteractable);

        if (!hitInteractable)
            return;

        if(Input.GetKeyDown(KeyCode.F))
        {
            interactable.Interact();
        }
    }
}
