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
        this.transform.position = new Vector3(setX, setY, setZ);
    }

    public void SetCornerElement()
    {
        bitMaskValue = BitMask.GetBitMask(nearGridElements);
        mesh.mesh = CornerMeshes.instance.GetCornerMesh(bitMaskValue, coord.y);
    }

    public void SetNearGridElements()
    {
        int width = LevelGenerator.width;
        int height = LevelGenerator.height;
        int length = LevelGenerator.length;

        if (coord.x < width && coord.y < height && coord.z < length){
            //UpperNorthEast
            nearGridElements[0] = LevelGenerator.instance.GetGridElement(coord.x, coord.y, coord.z);
        }

        if (coord.x > 0 && coord.y < height & coord.z < length) {
            //UpperNorthWest
            nearGridElements[1] = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y, coord.z);
        }

        if (coord.x > 0 && coord.y < height & coord.z > 0) {
            //UpperSouthWest
            nearGridElements[2] = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y, coord.z - 1);
        }

        if (coord.x < width && coord.y < height && coord.z > 0) {
            //UpperSouthEast
            nearGridElements[3] = LevelGenerator.instance.GetGridElement(coord.x, coord.y, coord.z - 1);
        }

        if (coord.x < width && coord.y > 0 && coord.z < length) {
            //LowerNorthEast
            nearGridElements[4] = LevelGenerator.instance.GetGridElement(coord.x, coord.y - 1, coord.z);
        }

        if (coord.x > 0 && coord.y > 0 & coord.z < length) {
            //LowerNorthWest
            nearGridElements[5] = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y - 1, coord.z);
        }

        if (coord.x > 0 && coord.y > 0 & coord.z > 0) {
            //LowerSouthWest
            nearGridElements[6] = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y - 1, coord.z - 1);
        }

        if (coord.x < width && coord.y > 0 && coord.z > 0) {
            //LowerSouthEast
            nearGridElements[7] = LevelGenerator.instance.GetGridElement(coord.x, coord.y - 1, coord.z - 1);
        }
        SetCornerElement();
    }
}
