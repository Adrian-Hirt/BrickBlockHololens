using System;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class MultiSelectionHandler : MonoBehaviour
{
    public static MultiSelectionHandler instance;
    private List<GridElement> selectedGridElements = new List<GridElement>();
    private Material selectionMaterial;

    private void Start()
    {
        selectionMaterial = Resources.Load<Material>("Selected_Delete");
        instance = this;
    }

    private void HandlePoseUpdate(MixedRealityPose pose, Handedness handedness)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pose.Position, 0f);

        if (hitColliders.Length > 0)
        {
            foreach (Collider hitCollider in hitColliders)
            {
                GridElement gridElement = hitCollider.gameObject.GetComponent<GridElement>();

                // Ignore objects which are not GridElements
                if (!gridElement)
                    continue;

                // Do not select ground elements
                if (gridElement.isGroundElement)
                    return;

                if (handedness == Handedness.Right)
                {
                    SelectGridElement(gridElement);
                }
                else
                {
                    DeselectGridElement(gridElement);
                }
            }
        }
    }

    private void Update()
    {
        // Do nothing if not in multi select mode
        if (PalmUpHandMenu.instance.gameMode != PalmUpHandMenu.GameMode.MultiSelectMode)
            return;

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose poseRight))
        {
            HandlePoseUpdate(poseRight, Handedness.Right);
        }
        else if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Left,
            out MixedRealityPose poseLeft))
        {
            HandlePoseUpdate(poseLeft, Handedness.Left);
        }
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
    
    public void RemoveSelectedElements()
    {
        foreach (GridElement gridElement in selectedGridElements)
        {
            DeselectGridElement(gridElement);
            gridElement.SetDisabled();
        }

        selectedGridElements.Clear();
    }
}
