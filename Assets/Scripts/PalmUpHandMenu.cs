using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class PalmUpHandMenu : MonoBehaviour
{
    public enum GameMode { LeftRightHandEditMode = 0, BuildDestroyButtonMode = 1, MoveMode = 2 };
    public GameMode gameMode;

    // BuildDestroyButtonMode states
    public enum EditMode { Build = 0, Destroy = 1 };
    public EditMode editMode;

    public GameObject level;
    public GameObject destroyToggleButton;
    public GameObject gameModeRadialSet;
    public GameObject buttonCursor;

    // Start is called before the first frame update
    void Start()
    {
        gameMode = GameMode.LeftRightHandEditMode;
        SetObjectManipulators(false);
    }


    public void SetEditMode()
    {
        if (destroyToggleButton.GetComponent<Interactable>().IsToggled)
        {
            editMode = EditMode.Destroy;
        }
        else
        {
            editMode = EditMode.Build;
        }
    }

    public void SetGameMode()
    {
        gameMode = (GameMode)gameModeRadialSet.GetComponent<InteractableToggleCollection>().CurrentIndex;
        switch (gameMode)
        {
            case GameMode.LeftRightHandEditMode:
                destroyToggleButton.SetActive(false);
                SetObjectManipulators(false);
                buttonCursor.SetActive(true);
                break;
            case GameMode.BuildDestroyButtonMode:
                destroyToggleButton.SetActive(true);
                SetObjectManipulators(false);
                buttonCursor.SetActive(true);
                break;
            case GameMode.MoveMode:
                destroyToggleButton.SetActive(false);
                SetObjectManipulators(true);
                buttonCursor.SetActive(false);
                break;
        }
    }
    private void SetObjectManipulators(bool active)
    {
        ObjectManipulator[] objectManipulators = level.GetComponentsInChildren<ObjectManipulator>();
        foreach (ObjectManipulator om in objectManipulators)
        {
            om.enabled = active;
        }
    }
}
