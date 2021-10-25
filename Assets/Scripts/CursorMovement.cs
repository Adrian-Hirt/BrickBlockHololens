using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;

public class CursorMovement : MonoBehaviour
{
    void Update()
    {
        foreach (var source in MixedRealityToolkit.InputSystem.DetectedInputSources)
        {
            // Ignore anything that is not a hand because we want articulated hands
            if (source.SourceType == Microsoft.MixedReality.Toolkit.Input.InputSourceType.Hand)
            {
                foreach (var p in source.Pointers)
                {
                    if (!(p is Microsoft.MixedReality.Toolkit.Input.ShellHandRayPointer))
                    {
                        // We only want the Microsoft.MixedReality.Toolkit.Input.ShellHandRayPointer
                        continue;
                    }

                    if (p.Result != null && p.Result.Details.Object != null &&
                        p.Result.Details.Object.tag == "gridElement")
                    {
                        if(!p.Result.Details.Object.gameObject.GetComponent<GridElement>().isGroundElement) {
                            // TODO: Disable "remove" action on ground level cubes
                        }
                        this.transform.position = p.Result.Details.Object.transform.position;
                        this.transform.localScale = p.Result.Details.Object.transform.localScale;
                    }
                }
            }
        }
    }
}