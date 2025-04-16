using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    private Material[][] originalMaterialsPerRenderer;
    private Renderer[] childRenderers;

    public Material highlightMaterial;

    void Start()
    {
        // Get all renderers in this object and its children
        childRenderers = GetComponentsInChildren<Renderer>();
        originalMaterialsPerRenderer = new Material[childRenderers.Length][];

        for (int i = 0; i < childRenderers.Length; i++)
        {
            originalMaterialsPerRenderer[i] = childRenderers[i].materials;
        }
    }

    public void Highlight()
    {
        foreach (var renderer in childRenderers)
        {
            Material[] highlightMats = new Material[renderer.materials.Length];
            for (int i = 0; i < highlightMats.Length; i++)
                highlightMats[i] = highlightMaterial;

            renderer.materials = highlightMats;
        }
    }

    public void Unhighlight()
    {
        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].materials = originalMaterialsPerRenderer[i];
        }
    }
}


