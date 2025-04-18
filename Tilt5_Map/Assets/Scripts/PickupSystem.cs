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

    public Transform rayPointer;

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.7f;
        lineRenderer.endWidth = 0.7f;
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
        lineRenderer.SetPosition(1, rayOrigin + rayDirection * 10f);

        if (Physics.Raycast(wandTransform.position, wandTransform.forward, out RaycastHit hit, 5f))
        {
            GameObject hitObj = hit.collider.gameObject;

            if (hitObj.CompareTag("Selectable"))
            {
                if (lastHoveredObject != hitObj)
                {
                    if (lastSelectable != null)
                        lastSelectable.Unhighlight();

                    lastHoveredObject = hitObj;
                    lastSelectable = hitObj.GetComponentInParent<Selectable>();

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

        Debug.DrawRay(wandTransform.position, wandTransform.forward * 2f, Color.red);

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



