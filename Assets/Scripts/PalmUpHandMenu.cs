using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalmUpHandMenu : MonoBehaviour
{
    public bool deleteMode;
    public enum EditMode { LeftRightHand, ModeToggleButtons }
    public EditMode editMode;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setDeleteMode()
    {
        deleteMode = true;
    }

    public void setBuildMode()
    {
        deleteMode = false;
    }

    public void setEditMode()
    {
        if (editMode == EditMode.LeftRightHand)
        {
            editMode = EditMode.ModeToggleButtons;
        }
        else
        {
            editMode = EditMode.LeftRightHand;
        }
    }
}
