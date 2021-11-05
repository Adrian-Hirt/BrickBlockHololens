using System;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class MultiSelectCursor : MonoBehaviour
{
    private PalmUpHandMenu palmMenu;
    private List<GridElement> selectedGridElements = new List<GridElement>();
    
    private void Start()
    {
        palmMenu = GameObject.FindGameObjectWithTag("GameMenu").GetComponent<PalmUpHandMenu>();
        this.gameObject.transform.localScale = Vector3.zero;
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
                    // Ignore objects which are not GridElements
                    if (!hitCollider.gameObject.GetComponent<GridElement>())
                    {
                        continue;
                    }

                    // TODO: Change material of selected elements instead of cursor
                    // TODO: Keep track of selected elements
                    // TODO: Add button to remove selected elements
                    
                    // Scale cursor correctly
                    Vector3 scaledScale = Vector3.Scale(
                        hitCollider.gameObject.transform.localScale,
                        new Vector3(LevelGenerator.scaleFactor, LevelGenerator.scaleFactor, LevelGenerator.scaleFactor)
                    );
                    
                    this.transform.localScale = scaledScale;

                    // Move cursor
                    this.transform.position = hitCollider.gameObject.transform.position;
                    break;
                }
            }
            else
            {
                this.gameObject.transform.localScale = Vector3.zero;
            }
        }
    }
}