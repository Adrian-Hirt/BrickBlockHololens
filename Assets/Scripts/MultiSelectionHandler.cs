using System;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class MultiSelectionHandler : MonoBehaviour
{
    public static MultiSelectionHandler instance;
    private PalmUpHandMenu palmMenu;
    private List<GridElement> selectedGridElements = new List<GridElement>();
    private Material selectionMaterial;
    private Material defaultMaterial;
    
    private void Start()
    {
        palmMenu = GameObject.FindGameObjectWithTag("GameMenu").GetComponent<PalmUpHandMenu>();
        selectionMaterial = Resources.Load<Material>("Selected");
        instance = this;
    }

    private void Update()
    {
        // Do nothing if not in multi select mode
        if (palmMenu.gameMode != PalmUpHandMenu.GameMode.MultiSelectMode)
            return;
        
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose))
        {
            Collider[] hitColliders = Physics.OverlapSphere(pose.Position, 0f);

            if (hitColliders.Length > 0)
            {
                foreach (Collider hitCollider in hitColliders)
                {
                    GridElement gridElement = hitCollider.gameObject.GetComponent<GridElement>();
                    
                    // Save default material so we can restore it
                    if (defaultMaterial == null)
                    {
                        defaultMaterial = gridElement.gameObject.GetComponent<Renderer>().material;
                    }
                    
                    // Ignore objects which are not GridElements
                    if (!gridElement)
                        continue;

                    // Do not select ground elements
                    if (gridElement.isGroundElement)
                        return;
                    
                    // Change material of selected elements
                    foreach (CornerElement cornerElement in gridElement.corners)
                    {
                        cornerElement.gameObject.GetComponent<Renderer>().material = selectionMaterial;
                    }
                    
                    // Keep track of selected grid elements
                    selectedGridElements.Add(gridElement);
                }
            }
        }
    }

    public void RemoveSelectedElements()
    {
        foreach (GridElement gridElement in selectedGridElements)
        {
            // Restore default material
            foreach (CornerElement cornerElement in gridElement.corners)
            {
                cornerElement.gameObject.GetComponent<Renderer>().material = defaultMaterial;
            }
            
            gridElement.SetDisabled();
        }
        
        selectedGridElements.Clear();
    }
}