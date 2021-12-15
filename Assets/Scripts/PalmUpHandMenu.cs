using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class PalmUpHandMenu : MonoBehaviour
{
    public static PalmUpHandMenu instance;
    public enum GameMode { PointerMode = 0, ColliderMode = 1, MoveScaleMode = 2, MultiDeleteMode = 3, ExtrusionMode = 4 };
    public GameMode gameMode;

    // BuildDestroyButtonMode states
    public enum EditMode { Build = 0, Destroy = 1, LeftRightHand = 2 };
    public EditMode editMode;

    public GameObject level;
    public GameObject destroyToggleButton;
    public GameObject gameModeRadialSet;
    public GameObject buttonCursor;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        gameMode = GameMode.PointerMode;
        editMode = EditMode.LeftRightHand;
        SetObjectManipulators(false);
        destroyToggleButton.SetActive(false);
    }

    public void SetEditMode()
    {
        switch (gameMode)
        {
            case GameMode.MultiDeleteMode:
                MultiDeleteHandler.instance.RemoveSelectedElements();
                break;
            default:
                if (destroyToggleButton.GetComponent<Interactable>().IsToggled)
                {
                    editMode = EditMode.Destroy;
                }
                else
                {
                    editMode = EditMode.Build;
                }
                break;
        }
    }

    public void SetGameMode()
    {
        MultiDeleteHandler.instance.ResetSelection();
        ExtrusionHandler.instance.ResetSelection();

        gameMode = (GameMode)gameModeRadialSet.GetComponent<InteractableToggleCollection>().CurrentIndex;
        switch (gameMode)
        {
            case GameMode.PointerMode:
                SetObjectManipulators(false);
                buttonCursor.SetActive(true);
                break;
            case GameMode.MoveScaleMode:
                SetObjectManipulators(true);
                buttonCursor.SetActive(false);
                break;
            case GameMode.MultiDeleteMode:
                SetObjectManipulators(false);
                buttonCursor.SetActive(false);
                break;
            case GameMode.ExtrusionMode:
                SetObjectManipulators(false);
                buttonCursor.SetActive(true);
                break;
            case GameMode.ColliderMode:
                SetObjectManipulators(false);
                buttonCursor.SetActive(false);
                break;
        }
    }
    public void SetUseDestroyButton()
    {
        if (editMode == EditMode.LeftRightHand)
        {
            editMode = EditMode.Build;
            destroyToggleButton.SetActive(true);
        }
        else
        {
            editMode = EditMode.LeftRightHand;
            destroyToggleButton.SetActive(false);
        }
    }
    public void SetMesh()
    {
        CornerMeshes.instance.UpdateMesh();
    }
    public bool ObjectManipulatorsActive()
    {
        return gameMode == GameMode.MoveScaleMode;
    }
    private void SetObjectManipulators(bool active)
    {
        ObjectManipulator[] objectManipulators = level.GetComponentsInChildren<ObjectManipulator>();
        foreach (ObjectManipulator om in objectManipulators)
        {
            om.enabled = active;
        }
        foreach (NearInteractionGrabbable grabbable in level.GetComponentsInChildren<NearInteractionGrabbable>())
        {
            grabbable.enabled = active;
        }
    }

}
