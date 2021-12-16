using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;

public class CursorMovement : MonoBehaviour
{
    public GridElement pointingAt;
    private Boolean visible = true;

    private void Show()
    {
        if (visible)
        {
            return;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        visible = true;
    }

    private void Hide()
    {
        if (!visible)
        {
            return;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        visible = false;
    }

    void Update()
    {
        ShellHandRayPointer pointer = PointerUtils.GetPointer<ShellHandRayPointer>(Handedness.Any);

        if (pointer != null && pointer.Result != null && pointer.Result.CurrentPointerTarget != null && pointer.Result.CurrentPointerTarget.CompareTag("gridElement"))
        {
            Show();
            GridElement currentElement = pointer.Result.CurrentPointerTarget.GetComponent<GridElement>();

            if (currentElement.isGroundElement)
            {
                return;
            }

            Transform elementTransform = currentElement.transform;

            pointingAt = currentElement;
            transform.localPosition = elementTransform.localPosition;
            transform.localScale = elementTransform.localScale + new Vector3(0.001f, 0.001f, 0.001f);

            return;
        }

        if (pointer != null && pointer.Result != null && pointer.Result.CurrentPointerTarget == null)
        {
            Hide();
        }
    }
}
