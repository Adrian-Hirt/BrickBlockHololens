using System;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class CopyPasteHandler : MonoBehaviour
{
    public static CopyPasteHandler instance;
    private HashSet<GridElement> selectedGridElements = new HashSet<GridElement>();
    private Material selectionMaterial;

    private void Start()
    {
        selectionMaterial = Resources.Load<Material>("Selected_Copy");
        instance = this;
    }
    
    private void Update()
    {
        
    }

    public void SelectGridElement(GridElement element)
    {
        Renderer rend = element.GetComponent<Renderer>();
        rend.enabled = true;
        rend.material = selectionMaterial;

        selectedGridElements.Add(element);
    }
    
    public void ResetSelection()
    {
        foreach (GridElement gridElement in selectedGridElements)
        {
            DeselectGridElement(gridElement);
        }

        selectedGridElements.Clear();
    }

    public void DeselectGridElement(GridElement element)
    {
        Renderer rend = element.GetComponent<Renderer>();
        rend.enabled = false;
        rend.material = null;
    }
}
