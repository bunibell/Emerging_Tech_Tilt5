using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    public Raycaster raycaster; // drag your raycaster object into this field in Inspector
    private GameObject heldObject = null;
    public Transform holdPoint; // an empty GameObject placed where you want to "hold" items

    void Update()
    {
        // Replace this with wand button later – using Mouse0 (left click) for now
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {
                DropObject();
            }
        }

        // Keep held object in front of wand
        if (heldObject != null)
        {
            heldObject.transform.position = holdPoint.position;
            heldObject.transform.rotation = holdPoint.rotation;
        }
    }

    void TryPickup()
    {
        if (raycaster.target != null && raycaster.target.CompareTag("Selectable"))
        {
            heldObject = raycaster.target;
            heldObject.GetComponent<Rigidbody>().isKinematic = true; // so it doesn’t fall
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject = null;
        }
    }
}
