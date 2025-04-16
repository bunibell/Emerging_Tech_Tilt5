using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TiltFive;
using static TiltFive.Input; 

public class PickupSystem : MonoBehaviour
{
    public Transform wandTransform; 

    private GameObject heldObject = null;
    private LineRenderer lineRenderer;

    private GameObject lastHoveredObject = null;
    private Selectable lastSelectable = null;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // Check if the wand button is pressed
        if (GetButtonDown(WandButton.One))
        {
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }

        Vector3 rayOrigin = wandTransform.position;
        Vector3 rayDirection = wandTransform.forward;

        lineRenderer.SetPosition(0, rayOrigin);
        lineRenderer.SetPosition(1, rayOrigin + rayDirection * 2f); // 2 units long ray

        if (Physics.Raycast(wandTransform.position, wandTransform.forward, out RaycastHit hit, 2f))
        {
            GameObject hitObj = hit.collider.gameObject;

            if (hitObj.CompareTag("Selectable"))
            {
                if (lastHoveredObject != hitObj)
                {
                    if (lastSelectable != null)
                        lastSelectable.Unhighlight();

                    lastHoveredObject = hitObj;
                    lastSelectable = hitObj.GetComponent<Selectable>();

                    if (lastSelectable != null)
                        lastSelectable.Highlight();
                }
            }
            else
            {
                // Not hovering over a Selectable
                if (lastSelectable != null)
                {
                    lastSelectable.Unhighlight();
                    lastSelectable = null;
                    lastHoveredObject = null;
                }
            }
        }
        else
        {
            // Nothing hit
            if (lastSelectable != null)
            {
                lastSelectable.Unhighlight();
                lastSelectable = null;
                lastHoveredObject = null;
            }
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



