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

    public void Drag(Vector3 dragDiff)
    {
        if (selectedGridElements.Count == 0)
        {
            return;
        }
        
        int dragX = (Int32) dragDiff.x;
        int dragY = (Int32) dragDiff.y;
        int dragZ = (Int32) dragDiff.z;

        if (dragX == 0 && dragY == 0 && dragZ == 0)
        {
            return;
        }

        if (Math.Abs(dragX) >= Math.Abs(dragY) && Math.Abs(dragX) >= Math.Abs(dragZ))
        {
            foreach (GridElement gridElement in selectedGridElements)
            {
                LevelGenerator.instance.GetGridElement(gridElement.GetCoord().x - dragX, gridElement.GetCoord().y, gridElement.GetCoord().z).SetTapEnabled();
            }
        }
        else if (Math.Abs(dragY) >= Math.Abs(dragX) && Math.Abs(dragY) >= Math.Abs(dragZ))
        {
            foreach (GridElement gridElement in selectedGridElements)
            {
                LevelGenerator.instance.GetGridElement(gridElement.GetCoord().x, gridElement.GetCoord().y - dragY, gridElement.GetCoord().z).SetTapEnabled();
            }
        }
        else if (Math.Abs(dragZ) >= Math.Abs(dragX) && Math.Abs(dragZ) >= Math.Abs(dragY))
        {
            foreach (GridElement gridElement in selectedGridElements)
            {
                LevelGenerator.instance.GetGridElement(gridElement.GetCoord().x, gridElement.GetCoord().y, gridElement.GetCoord().z - dragZ).SetTapEnabled();
            }
        }
    }
}
