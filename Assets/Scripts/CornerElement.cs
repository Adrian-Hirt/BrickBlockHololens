using UnityEngine;

public class CornerElement : MonoBehaviour
{
    private coord coord;
    public GridElement[] nearGridElements;
    public int bitMaskValue;
    private MeshFilter mesh;

    public void Initialize(int setX, int setY, int setZ)
    {
        coord = new coord(setX, setY, setZ);
        nearGridElements = new GridElement[8];
        this.name = "CE_" + coord.x + "_" + coord.y + "_" + coord.z;
        mesh = this.GetComponent<MeshFilter>();
    }

    public void SetPosition(float setX, float setY, float setZ)
    {
        this.transform.localPosition = new Vector3(setX, setY, setZ);
    }

    public void SetCornerElement()
    {
        bitMaskValue = BitMask.GetBitMask(nearGridElements);
        mesh.mesh = CornerMeshes.instance.GetCornerMesh(bitMaskValue, coord.y);
    }

    public void SetNearGridElements()
    {
        if (coord.x < LevelGenerator.currentWidthHigh && coord.y < LevelGenerator.currentHeightHigh && coord.z < LevelGenerator.currentLengthHigh)
        {
            //UpperNorthEast
            nearGridElements[0] = LevelGenerator.instance.GetGridElement(coord.x, coord.y, coord.z);
        }

        if (coord.x > LevelGenerator.currentWidthLow && coord.y < LevelGenerator.currentHeightHigh & coord.z < LevelGenerator.currentLengthHigh)
        {
            //UpperNorthWest
            nearGridElements[1] = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y, coord.z);
        }

        if (coord.x > LevelGenerator.currentWidthLow && coord.y < LevelGenerator.currentHeightHigh & coord.z > LevelGenerator.currentLengthLow)
        {
            //UpperSouthWest
            nearGridElements[2] = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y, coord.z - 1);
        }

        if (coord.x < LevelGenerator.currentWidthHigh && coord.y < LevelGenerator.currentHeightHigh && coord.z > LevelGenerator.currentLengthLow)
        {
            //UpperSouthEast
            nearGridElements[3] = LevelGenerator.instance.GetGridElement(coord.x, coord.y, coord.z - 1);
        }

        if (coord.x < LevelGenerator.currentWidthHigh && coord.y > 0 && coord.z < LevelGenerator.currentLengthHigh)
        {
            //LowerNorthEast
            nearGridElements[4] = LevelGenerator.instance.GetGridElement(coord.x, coord.y - 1, coord.z);
        }

        if (coord.x > LevelGenerator.currentWidthLow && coord.y > 0 & coord.z < LevelGenerator.currentLengthHigh)
        {
            //LowerNorthWest
            nearGridElements[5] = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y - 1, coord.z);
        }

        if (coord.x > LevelGenerator.currentWidthLow && coord.y > 0 & coord.z > LevelGenerator.currentLengthLow)
        {
            //LowerSouthWest
            nearGridElements[6] = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y - 1, coord.z - 1);
        }

        if (coord.x < LevelGenerator.currentWidthHigh && coord.y > 0 && coord.z > LevelGenerator.currentLengthLow)
        {
            //LowerSouthEast
            nearGridElements[7] = LevelGenerator.instance.GetGridElement(coord.x, coord.y - 1, coord.z - 1);
        }
        SetCornerElement();
    }
}
