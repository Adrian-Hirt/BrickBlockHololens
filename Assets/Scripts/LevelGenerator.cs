using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;
    public static int width = 5;
    public static int height = 5;
    public GridElement gridElement;
    public CornerElement cornerElement;
    private GridElement[,,] gridElements;
    public CornerElement[] cornerElements;

    // Factor with which the model should be scaled, NEEDS to be equal
    // to the size of the LevelGenerator in Unity!
    public static float scaleFactor = 0.1f;

    // Offset of the model in x, y and z direction when initially rendered
    private int xOffset = 2;
    private int zOffset = -1;
    private int yOffset = -2;

    private float floorHeight = 0.25f, basementHeight;
    void Start()
    {
        this.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        instance = this;
        basementHeight = 1.5f - floorHeight / 2;
        float elementHeight;
        gridElements = new GridElement[width, height, width]; // Access by x, y, z
        cornerElements = new CornerElement[(width + 1) * (width + 1) * (height + 1)];
        for (int y = 0; y < height + 1; y++)
        {
            for (int x = 0; x < width + 1; x++)
            {
                for (int z = 0; z < width + 1; z++)
                {
                    CornerElement cornerElementInstance = Instantiate(cornerElement, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
                    cornerElementInstance.GetComponent<ObjectManipulator>().HostTransform = this.transform.parent.transform;
                    cornerElementInstance.Initialize(x, y, z);
                    cornerElements[x + (width + 1) * (z + (width + 1) * y)] = cornerElementInstance;
                }
            }
        }

        for (int y = 0; y < height; y++)
        {
            float yPos = y;
            if (y == 0)
            {
                elementHeight = floorHeight;
            }
            else if (y == 1)
            {
                elementHeight = basementHeight;
                yPos = floorHeight / 2 + basementHeight / 2;
            }
            else
            {
                elementHeight = 1;
            }
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < width; z++)
                {
                    Vector3 scaledPosition = Vector3.Scale(new Vector3(x + xOffset, yPos + yOffset, z + zOffset), new Vector3(scaleFactor, scaleFactor, scaleFactor));

                    GridElement gridElementInstance = Instantiate(gridElement, scaledPosition, Quaternion.identity, this.transform);
                    gridElementInstance.GetComponent<ObjectManipulator>().HostTransform = this.transform.parent.transform;
                    gridElementInstance.tag = "gridElement";
                    gridElementInstance.Initialize(x, y, z, elementHeight);
                    this.SetGridElement(x, y, z, gridElementInstance);
                }
            }
        }
        Physics.SyncTransforms();

        foreach (CornerElement corner in cornerElements)
        {
            corner.SetNearGridElements();
        }
        foreach (GridElement ge in gridElements)
        {
            ge.SetCornerPositions();
            ge.SetEnabled();
        }
    }

    public void SetGridElement(int x, int y, int z, GridElement element)
    {
        gridElements[x, y, z] = element;
    }

    public GridElement GetGridElement(int x, int y, int z)
    {
        return gridElements[x, y, z];
    }

    public void UpdateScale(Microsoft.MixedReality.Toolkit.UI.SliderEventData data)
    {
        float newValue = data.NewValue;
        scaleFactor = newValue;

        if (scaleFactor > 0.0f)
        {
            this.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
    }
}
