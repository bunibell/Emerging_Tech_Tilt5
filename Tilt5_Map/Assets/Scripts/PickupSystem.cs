using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TiltFive;
using static TiltFive.Input; 

public class PickupSystem : MonoBehaviour
{
    public Transform wandTransform; // Assign this in the Inspector

    private GameObject heldObject = null;

    void Update()
    {
        // Check if the wand button is pressed using the static API
        if (GetButtonDown(WandButton.One))
        {
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }
    }

    void TryPickup()
    {
        if (Physics.Raycast(wandTransform.position, wandTransform.forward, out RaycastHit hit, 2f))
        {
            if (hit.collider.CompareTag("Selectable"))
            {
                heldObject = hit.collider.gameObject;
                heldObject.transform.SetParent(wandTransform);
                heldObject.transform.localPosition = new Vector3(0, 0, 0.1f);
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            heldObject = null;
        }
    }
}



