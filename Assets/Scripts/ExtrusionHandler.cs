using System;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEditor;
using UnityEngine;

public class ExtrusionHandler : MonoBehaviour
{
    public static ExtrusionHandler instance;
    private HashSet<GridElement> selectedGridElements = new HashSet<GridElement>();
    private Material selectionMaterial;
    private Boolean isGrabbing = true;
    private Vector3 dragStart;
    private Handedness grabbingHand = Handedness.None;

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
                    DeselectGridElement(gridElement, true);
                }
            }
        }

        if (selectedGridElements.Count <= 0)
        {
            return;
        }
        
        if (isGrabbing && handedness == grabbingHand)
        {
            Vector3 dragDiff = (dragStart - pose.Position).Mul(new Vector3(10, 10, 10));
            
            // Rotate by level rotation
            dragDiff = Quaternion.Inverse(gameObject.transform.rotation) * dragDiff;
            
            Drag(dragDiff);
        }

        Boolean leftGrabbing = HandPoseUtils.IsIndexGrabbing(Handedness.Left) ||
                                    HandPoseUtils.IsMiddleGrabbing(Handedness.Left) ||
                                    HandPoseUtils.IsThumbGrabbing(Handedness.Left);
        
        Boolean rightGrabbing = HandPoseUtils.IsIndexGrabbing(Handedness.Right) ||
                                    HandPoseUtils.IsMiddleGrabbing(Handedness.Right) ||
                                    HandPoseUtils.IsThumbGrabbing(Handedness.Right);

        if (!isGrabbing && (leftGrabbing || rightGrabbing))
        {
            isGrabbing = true;
            dragStart = pose.Position;
            grabbingHand = handedness;
        }
        else if (isGrabbing && (!leftGrabbing && grabbingHand == Handedness.Left || !rightGrabbing && grabbingHand == Handedness.Right))
        {
            isGrabbing = false;
            grabbingHand = Handedness.None;

            Vector3 dragDiff = (dragStart - pose.Position).Mul(new Vector3(10, 10, 10));
            if (Math.Abs(dragDiff.x) >= 1 || Math.Abs(dragDiff.y) >= 1 || Math.Abs(dragDiff.z) >= 1)
            {
                ResetSelection();
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
            DeselectGridElement(gridElement, false);
        }

        selectedGridElements.Clear();
    }

    public void DeselectGridElement(GridElement element, bool removeElement)
    {
        Renderer rend = element.GetComponent<Renderer>();
        rend.enabled = false;
        rend.material = null;
        
        if (removeElement)
            selectedGridElements.Remove(element);
    }

    public void Drag(Vector3 dragDiff)
    {
        if (selectedGridElements.Count == 0)
        {
            return;
        }

        int dragX = (Int32)dragDiff.x;
        int dragY = (Int32)dragDiff.y;
        int dragZ = (Int32)dragDiff.z;

        if (dragX == 0 && dragY == 0 && dragZ == 0)
        {
            return;
        }

        if (Math.Abs(dragX) >= Math.Abs(dragY) && Math.Abs(dragX) >= Math.Abs(dragZ))
        {
            foreach (GridElement gridElement in selectedGridElements)
            {
                LevelGenerator.instance.GetGridElement(gridElement.GetCoord().x - dragX, gridElement.GetCoord().y,
                    gridElement.GetCoord().z)?.SetTapEnabled();
            }
        }
        else if (Math.Abs(dragY) >= Math.Abs(dragX) && Math.Abs(dragY) >= Math.Abs(dragZ))
        {
            foreach (GridElement gridElement in selectedGridElements)
            {
                LevelGenerator.instance.GetGridElement(gridElement.GetCoord().x, gridElement.GetCoord().y - dragY,
                    gridElement.GetCoord().z)?.SetTapEnabled();
            }
        }
        else if (Math.Abs(dragZ) >= Math.Abs(dragX) && Math.Abs(dragZ) >= Math.Abs(dragY))
        {
            foreach (GridElement gridElement in selectedGridElements)
            {
                LevelGenerator.instance.GetGridElement(gridElement.GetCoord().x, gridElement.GetCoord().y,
                    gridElement.GetCoord().z - dragZ)?.SetTapEnabled();
            }
        }
    }
}