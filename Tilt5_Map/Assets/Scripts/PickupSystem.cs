using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TiltFive;
using static TiltFive.Input;

public class PickupSystem : MonoBehaviour
{
    public Transform wandTransform; // Drag your wand GameObject’s Transform here
    public Player player;           // Drag your TiltFivePlayer GameObject here

    private GameObject heldObject = null;

    void Update()
    {
        if (player != null && player.GetWandDevice().GetButtonDown(WandButton.One))
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
                heldObject.transform.localPosition = new Vector3(0, 0, 0.1f); // Slight offset
                heldObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    void DropObject()
    {
        heldObject.transform.SetParent(null);
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject = null;
    }
}
