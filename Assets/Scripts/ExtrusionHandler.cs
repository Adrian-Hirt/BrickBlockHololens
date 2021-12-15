using System;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class ExtrusionHandler : MonoBehaviour
{
    public static ExtrusionHandler instance;
    private HashSet<GridElement> selectedGridElements = new HashSet<GridElement>();
    private Material selectionMaterial;

    private void Start()
    {
        selectionMaterial = Resources.Load<Material>("Selected_Copy");
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
        if (PalmUpHandMenu.instance.gameMode != PalmUpHandMenu.GameMode.ExtrusionMode)
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
                LevelGenerator.instance.GetGridElement(gridElement.GetCoord().x - dragX, gridElement.GetCoord().y, gridElement.GetCoord().z)?.SetTapEnabled();
            }
        }
        else if (Math.Abs(dragY) >= Math.Abs(dragX) && Math.Abs(dragY) >= Math.Abs(dragZ))
        {
            foreach (GridElement gridElement in selectedGridElements)
            {
                LevelGenerator.instance.GetGridElement(gridElement.GetCoord().x, gridElement.GetCoord().y - dragY, gridElement.GetCoord().z)?.SetTapEnabled();
            }
        }
        else if (Math.Abs(dragZ) >= Math.Abs(dragX) && Math.Abs(dragZ) >= Math.Abs(dragY))
        {
            foreach (GridElement gridElement in selectedGridElements)
            {
                LevelGenerator.instance.GetGridElement(gridElement.GetCoord().x, gridElement.GetCoord().y, gridElement.GetCoord().z - dragZ)?.SetTapEnabled();
            }
        }
    }
}
