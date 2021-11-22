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
    private Material defaultMaterial;

    private void Start()
    {
        selectionMaterial = Resources.Load<Material>("Selected");
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

                // Save default material so we can restore it
                if (defaultMaterial == null)
                {
                    defaultMaterial = gridElement.gameObject.GetComponent<Renderer>().material;
                }

                // Do not select ground elements
                if (gridElement.isGroundElement)
                    return;

                if (handedness == Handedness.Right)
                {
                    // Change material of selected elements
                    foreach (CornerElement cornerElement in gridElement.corners)
                    {
                        cornerElement.gameObject.GetComponent<Renderer>().material = selectionMaterial;
                    }

                    // Keep track of selected grid elements
                    selectedGridElements.Add(gridElement);
                }
                else
                {
                    // Change material of selected elements
                    foreach (CornerElement cornerElement in gridElement.corners)
                    {
                        cornerElement.gameObject.GetComponent<Renderer>().material = defaultMaterial;
                    }

                    // Keep track of selected grid elements
                    selectedGridElements.Remove(gridElement);
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

    public void ResetSelection()
    {
        if (defaultMaterial == null)
            return;

        foreach (GridElement gridElement in selectedGridElements)
        {
            // Restore default material
            foreach (CornerElement cornerElement in gridElement.corners)
            {
                cornerElement.gameObject.GetComponent<Renderer>().material = defaultMaterial;
            }
        }

        selectedGridElements.Clear();
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
