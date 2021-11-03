using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;
    public static int width = 5; // X Direction
    public static int height = 5; // Y direction
    public static int length = 3; // Z direction

    public static int currentWidthLow, currentWidthHigh;
    public static int currentLengthLow, currentLengthHigh;
    public static int currentHeightLow, currentHeightHigh;

    public GridElement gridElement;
    public CornerElement cornerElement;

    public static float scaleFactor = 0.1f;

    // Offset of the model in x, y and z direction when initially rendered
    private int xOffset = 2;
    private int zOffset = -1;
    private int yOffset = -2;

    private Dictionary<string, GridElement> gridElements;
    private Dictionary<string, CornerElement> cornerElementsDict;

    private float floorHeight = 0.25f, basementHeight;

    void Start() {
        gridElements = new Dictionary<string, GridElement>();
        cornerElementsDict = new Dictionary<string, CornerElement>();

        this.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        instance = this;
        basementHeight = 1.5f - floorHeight / 2;
        float elementHeight;

        currentWidthLow = 0;
        currentWidthHigh = width;

        currentLengthLow = 0;
        currentLengthHigh = length;

        currentHeightLow = 0;
        currentHeightHigh = height;

        for (int y = 0; y < height + 1; y++) {
            for (int x = 0; x < width + 1; x++) {
                for (int z = 0; z < length + 1; z++) {
                    CornerElement cornerElementInstance = Instantiate(cornerElement, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
                    cornerElementInstance.GetComponent<ObjectManipulator>().HostTransform = this.transform.parent.transform;
                    cornerElementInstance.Initialize(x, y, z);
                    this.SetCornerElement(x, y, z, cornerElementInstance);
                }
            }
        }

        for (int y = 0; y < height; y++) {
            float yPos = y;
            if (y == 0) {
                elementHeight = floorHeight;
            }
            else if (y == 1) {
                elementHeight = basementHeight;
                yPos = floorHeight / 2 + basementHeight / 2;
            }
            else {
                elementHeight = 1;
            }
            for (int x = 0; x < width; x++) {
                for (int z = 0; z < length; z++) {
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


        for (int y = 0; y < height + 1; y++) {
            for (int x = 0; x < width + 1; x++) {
                for (int z = 0; z < length + 1; z++) {
                    this.GetCornerElement(x, y, z).SetNearGridElements();
                }
            }
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                for (int z = 0; z < length; z++) {
                    GridElement ge = this.GetGridElement(x, y, z);
                    ge.SetCornerPositions();

                    // "Outer shell", disabled
                    if (y != 0 && (x == width - 1 || x == 0 || z == 0 || z == length - 1 || y == height - 1)) {
                        ge.SetDisabled();
                    }
                    else {
                        ge.SetEnabled();
                    }
                }
            }
        }
    }

    public void UpdateScale(Microsoft.MixedReality.Toolkit.UI.SliderEventData data) {
        float newValue = data.NewValue;
        scaleFactor = newValue;

        if (scaleFactor > 0.0f) {
            this.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
    }

    private string CoordinatesToDictkey(int x, int y, int z) {
        return x.ToString() + "/" + y.ToString() + "/" + z.ToString();
    }

    public void SetGridElement(int x, int y, int z, GridElement element) {
        string key = this.CoordinatesToDictkey(x, y, z);
        gridElements.Add(key, element);
    }

    public GridElement GetGridElement(int x, int y, int z) {
        string key = this.CoordinatesToDictkey(x, y, z);
        return gridElements[key];
    }

    public void SetCornerElement(int x, int y, int z, CornerElement element) {
        string key = this.CoordinatesToDictkey(x, y, z);
        cornerElementsDict.Add(key, element);
    }

    public CornerElement GetCornerElement(int x, int y, int z) {
        string key = this.CoordinatesToDictkey(x, y, z);
        return cornerElementsDict[key];
    }

    public void AddShellInDirectionX(bool negativeX) {
        int cornerElementX;
        int gridElementX;
        int otherCornerElementX;

        if(negativeX) {
            currentWidthLow -= 1;
            gridElementX = currentWidthLow;
            cornerElementX = currentWidthLow;
            otherCornerElementX = gridElementX + 1;
        }
        else {
            gridElementX = currentWidthHigh;
            otherCornerElementX = currentWidthHigh;
            currentWidthHigh += 1;
            cornerElementX = currentWidthHigh;
        }

        float elementHeight;

        for (int y = 0; y < currentHeightHigh + 1; y++) {
            for (int z = currentLengthLow; z < currentLengthHigh + 1; z++) {
                CornerElement cornerElementInstance = Instantiate(cornerElement, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
                cornerElementInstance.GetComponent<ObjectManipulator>().HostTransform = this.transform.parent.transform;
                cornerElementInstance.Initialize(cornerElementX, y, z);
                this.SetCornerElement(cornerElementX, y, z, cornerElementInstance);
            }
        }

        for (int y = 0; y < currentHeightHigh; y++) {
            float yPos = y;
            if (y == 0) {
                elementHeight = floorHeight;
            }
            else if (y == 1) {
                elementHeight = basementHeight;
                yPos = floorHeight / 2 + basementHeight / 2;
            }
            else {
                elementHeight = 1;
            }
            for (int z = currentLengthLow; z < currentLengthHigh; z++) {
                Vector3 scaledPosition = Vector3.Scale(new Vector3(gridElementX + xOffset, yPos + yOffset, z + zOffset), new Vector3(scaleFactor, scaleFactor, scaleFactor));

                GridElement gridElementInstance = Instantiate(gridElement, scaledPosition, Quaternion.identity, this.transform);
                gridElementInstance.GetComponent<ObjectManipulator>().HostTransform = this.transform.parent.transform;
                gridElementInstance.tag = "gridElement";
                gridElementInstance.Initialize(gridElementX, y, z, elementHeight);
                this.SetGridElement(gridElementX, y, z, gridElementInstance);
            }
        }

        Physics.SyncTransforms();

        for (int y = 0; y < currentHeightHigh + 1; y++) {
            for (int z = currentLengthLow; z < currentLengthHigh + 1; z++) {
                this.GetCornerElement(cornerElementX, y, z).SetNearGridElements();
                this.GetCornerElement(otherCornerElementX, y, z).SetNearGridElements();
            }
        } 

        for (int y = 0; y < currentHeightHigh; y++) {
            for (int z = currentLengthLow; z < currentLengthHigh; z++) {
                GridElement ge = this.GetGridElement(gridElementX, y, z);
                ge.SetCornerPositions();

                if(y != 0) {
                    ge.SetDisabled();
                }
                else {
                    ge.SetEnabled();
                }
            }
        }
    }

    public void AddShellInDirectionZ(bool negativeZ) {
        int cornerElementZ;
        int gridElementZ;
        int otherCornerElementZ;

        if(negativeZ) {
            currentLengthLow -= 1;
            gridElementZ = currentLengthLow;
            cornerElementZ = currentLengthLow;
            otherCornerElementZ = gridElementZ + 1;
        }
        else {
            gridElementZ = currentLengthHigh;
            otherCornerElementZ = currentLengthHigh;
            currentLengthHigh += 1;
            cornerElementZ = currentLengthHigh;
        }

        float elementHeight;

        for (int y = 0; y < currentHeightHigh + 1; y++) {
            for (int x = currentWidthLow; x < currentWidthHigh + 1; x++) {
                CornerElement cornerElementInstance = Instantiate(cornerElement, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
                cornerElementInstance.GetComponent<ObjectManipulator>().HostTransform = this.transform.parent.transform;
                cornerElementInstance.Initialize(x, y, cornerElementZ);
                this.SetCornerElement(x, y, cornerElementZ, cornerElementInstance);
            }
        }

        for (int y = 0; y < currentHeightHigh; y++) {
            float yPos = y;
            if (y == 0) {
                elementHeight = floorHeight;
            }
            else if (y == 1) {
                elementHeight = basementHeight;
                yPos = floorHeight / 2 + basementHeight / 2;
            }
            else {
                elementHeight = 1;
            }
            for (int x = currentWidthLow; x < currentWidthHigh; x++) {
                Vector3 scaledPosition = Vector3.Scale(new Vector3(x + xOffset, yPos + yOffset, gridElementZ + zOffset), new Vector3(scaleFactor, scaleFactor, scaleFactor));

                GridElement gridElementInstance = Instantiate(gridElement, scaledPosition, Quaternion.identity, this.transform);
                gridElementInstance.GetComponent<ObjectManipulator>().HostTransform = this.transform.parent.transform;
                gridElementInstance.tag = "gridElement";
                gridElementInstance.Initialize(x, y, gridElementZ, elementHeight);
                this.SetGridElement(x, y, gridElementZ, gridElementInstance);
            }
        }

        Physics.SyncTransforms();

        for (int y = 0; y < currentHeightHigh + 1; y++) {
            for (int x = currentWidthLow; x < currentWidthHigh + 1; x++) {
                this.GetCornerElement(x, y, cornerElementZ).SetNearGridElements();
                this.GetCornerElement(x, y, otherCornerElementZ).SetNearGridElements();
            }
        } 

        for (int y = 0; y < currentHeightHigh; y++) {
            for (int x = currentWidthLow; x < currentWidthHigh; x++) {
                GridElement ge = this.GetGridElement(x, y, gridElementZ);
                ge.SetCornerPositions();

                if(y != 0) {
                    ge.SetDisabled();
                }
                else {
                    ge.SetEnabled();
                }
            }
        }
    }

    public void AddShellInDirectionY() {
        int cornerElementY;
        int gridElementY;
        int otherCornerElementY;


        gridElementY = currentHeightHigh;
        otherCornerElementY = currentHeightHigh;
        currentHeightHigh += 1;
        cornerElementY = currentHeightHigh;

        float elementHeight;

        for (int z = currentLengthLow; z < currentLengthHigh + 1; z++) {
            for (int x = currentWidthLow; x < currentWidthHigh + 1; x++) {
                CornerElement cornerElementInstance = Instantiate(cornerElement, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
                cornerElementInstance.GetComponent<ObjectManipulator>().HostTransform = this.transform.parent.transform;
                cornerElementInstance.Initialize(x, cornerElementY, z);
                this.SetCornerElement(x, cornerElementY, z, cornerElementInstance);
            }
        }

        for (int z = currentLengthLow; z < currentLengthHigh; z++) {
            for (int x = currentWidthLow; x < currentWidthHigh; x++) {
                Vector3 scaledPosition = Vector3.Scale(new Vector3(x + xOffset, gridElementY + yOffset, z + zOffset), new Vector3(scaleFactor, scaleFactor, scaleFactor));

                GridElement gridElementInstance = Instantiate(gridElement, scaledPosition, Quaternion.identity, this.transform);
                gridElementInstance.GetComponent<ObjectManipulator>().HostTransform = this.transform.parent.transform;
                gridElementInstance.tag = "gridElement";
                gridElementInstance.Initialize(x, gridElementY, z, 1);
                this.SetGridElement(x, gridElementY, z, gridElementInstance);
            }
        }

        Physics.SyncTransforms();

        for (int z = currentLengthLow; z < currentLengthHigh + 1; z++) {
            for (int x = currentWidthLow; x < currentWidthHigh + 1; x++) {
                this.GetCornerElement(x, cornerElementY, z).SetNearGridElements();
                this.GetCornerElement(x, otherCornerElementY, z).SetNearGridElements();
            }
        } 

        for (int z = currentLengthLow; z < currentLengthHigh; z++) {
            for (int x = currentWidthLow; x < currentWidthHigh; x++) {
                GridElement ge = this.GetGridElement(x, gridElementY, z);
                ge.SetCornerPositions();
                ge.SetDisabled();
            }
        }
    }
}
