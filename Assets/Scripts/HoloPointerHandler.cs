using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;

public class HoloPointerHandler : BaseInputHandler, IMixedRealityPointerHandler
{
    private Vector3 dragStart;
    
    protected override void RegisterHandlers()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
    }

    protected override void UnregisterHandlers()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityPointerHandler>(this);
    }

    void IMixedRealityPointerHandler.OnPointerDown(MixedRealityPointerEventData eventData)
    {
        if (PalmUpHandMenu.instance.gameMode != PalmUpHandMenu.GameMode.CopyPasteMode)
        {
            return;
        }
        
        // Get grid element we're currently pointing at
        GridElement pointingAt = this.transform.parent.gameObject.GetComponent<CursorMovement>().pointingAt;
        ShellHandRayPointer pointer = (ShellHandRayPointer) eventData.Pointer;

        dragStart = pointer.transform.position;
    }

    void IMixedRealityPointerHandler.OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        if (PalmUpHandMenu.instance.gameMode != PalmUpHandMenu.GameMode.CopyPasteMode)
        {
            return;
        }
        
        // Get grid element we're currently pointing at
        GridElement pointingAt = this.transform.parent.gameObject.GetComponent<CursorMovement>().pointingAt;
        ShellHandRayPointer pointer = (ShellHandRayPointer) eventData.Pointer;
        Vector3 dragDiff = (dragStart - pointer.transform.position).Mul(new Vector3(10, 10, 10));

        CopyPasteHandler.instance.Drag(dragDiff);
    }
    
    void IMixedRealityPointerHandler.OnPointerUp(MixedRealityPointerEventData eventData)
    {
        if (PalmUpHandMenu.instance.gameMode != PalmUpHandMenu.GameMode.CopyPasteMode)
        {
            return;
        }
        
        ShellHandRayPointer pointer = (ShellHandRayPointer) eventData.Pointer;
        Vector3 dragDiff = (dragStart - pointer.transform.position).Mul(new Vector3(10, 10, 10));
        if (Math.Abs(dragDiff.x) >= 1 || Math.Abs(dragDiff.y) >= 1 || Math.Abs(dragDiff.z) >= 1)
        {
            CopyPasteHandler.instance.ResetSelection();
        }
    }

    void IMixedRealityPointerHandler.OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        bool deleteMode = false;
        switch (PalmUpHandMenu.instance.gameMode)
        {
            case PalmUpHandMenu.GameMode.CopyPasteMode:
            case PalmUpHandMenu.GameMode.PointerMode:
                if (PalmUpHandMenu.instance.editMode == PalmUpHandMenu.EditMode.LeftRightHand)
                {
                    deleteMode = eventData.Handedness == Microsoft.MixedReality.Toolkit.Utilities.Handedness.Left;
                }
                else
                {
                    deleteMode = PalmUpHandMenu.instance.editMode == PalmUpHandMenu.EditMode.Destroy;
                }

                break;
            default:
                return;
        }

        if (!eventData.used)
        {
            // Get grid element we're currently pointing at
            GridElement pointingAt = this.transform.parent.gameObject.GetComponent<CursorMovement>().pointingAt;

            if (PalmUpHandMenu.instance.gameMode == PalmUpHandMenu.GameMode.CopyPasteMode)
            {
                if (deleteMode)
                {
                    CopyPasteHandler.instance.DeselectGridElement(pointingAt);
                }
                else
                {
                    CopyPasteHandler.instance.SelectGridElement(pointingAt);
                }

                eventData.Use();
                return;
            }

            if (deleteMode)
            {
                pointingAt.SetDisabled();
            }
            else
            {
                coord coord = pointingAt.GetCoord();

                GridElement otherElement;

                try
                {
                    switch (this.name)
                    {
                        case "Button_X_Pos":
                            otherElement = LevelGenerator.instance.GetGridElement(coord.x + 1, coord.y, coord.z);
                            break;
                        case "Button_X_Neg":
                            otherElement = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y, coord.z);
                            break;
                        case "Button_Y_Pos":
                            otherElement = LevelGenerator.instance.GetGridElement(coord.x, coord.y + 1, coord.z);
                            break;
                        case "Button_Y_Neg":
                            otherElement = LevelGenerator.instance.GetGridElement(coord.x, coord.y - 1, coord.z);
                            break;
                        case "Button_Z_Pos":
                            otherElement = LevelGenerator.instance.GetGridElement(coord.x, coord.y, coord.z + 1);
                            break;
                        case "Button_Z_Neg":
                            otherElement = LevelGenerator.instance.GetGridElement(coord.x, coord.y, coord.z - 1);
                            break;
                        default:
                            otherElement = null;
                            break;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    otherElement = null;
                }

                // Use null-safe operator
                otherElement?.SetTapEnabled();
            }
            eventData.Use();
        }
    }
}
