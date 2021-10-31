using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;

public class HoloPointerHandler : BaseInputHandler, IMixedRealityPointerHandler
{
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
        // Do nothing for now
    }

    void IMixedRealityPointerHandler.OnPointerUp(MixedRealityPointerEventData eventData)
    {
        // Do nothing for now
    }

    void IMixedRealityPointerHandler.OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        bool deleteMode;
        PalmUpHandMenu palmMenu = GameObject.FindGameObjectWithTag("GameMenu").GetComponent<PalmUpHandMenu>();
        if (palmMenu.editMode == PalmUpHandMenu.EditMode.LeftRightHand)
        {
            deleteMode = eventData.Handedness == Microsoft.MixedReality.Toolkit.Utilities.Handedness.Left;
        }
        else
        {
            deleteMode = palmMenu.deleteMode;
        }

        if (!eventData.used)
        {
            // Get grid element we're currently pointing at
            GridElement pointingAt = this.transform.parent.gameObject.GetComponent<CursorMovement>().pointingAt;

            if (deleteMode)
            {
                pointingAt.SetDisabled();
            }
            else
            {
                coord coord = pointingAt.GetCoord();
                int height = LevelGenerator.height;
                int width = LevelGenerator.width;

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
                otherElement?.SetEnabled();
            }
            eventData.Use();
        }
    }

    void IMixedRealityPointerHandler.OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        // Do nothing for now
    }
}
