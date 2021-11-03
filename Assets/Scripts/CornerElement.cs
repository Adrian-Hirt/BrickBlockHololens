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

        if (coord.x < LevelGenerator.currentCornerElementMaxX && coord.y < height && coord.z < LevelGenerator.currentCornerElementMaxZ){
            //UpperNorthEast
            nearGridElements[0] = LevelGenerator.instance.GetGridElement(coord.x, coord.y, coord.z);
        }

        if (coord.x > LevelGenerator.currentCornerElementMinX && coord.y < height & coord.z < LevelGenerator.currentCornerElementMaxZ) {
            //UpperNorthWest
            nearGridElements[1] = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y, coord.z);
        }

        if (coord.x > LevelGenerator.currentCornerElementMinX && coord.y < height & coord.z > LevelGenerator.currentCornerElementMinZ) {
            //UpperSouthWest
            nearGridElements[2] = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y, coord.z - 1);
        }

        if (coord.x < LevelGenerator.currentCornerElementMaxX && coord.y < height && coord.z > LevelGenerator.currentCornerElementMinZ) {
            //UpperSouthEast
            nearGridElements[3] = LevelGenerator.instance.GetGridElement(coord.x, coord.y, coord.z - 1);
        }

        if (coord.x < LevelGenerator.currentCornerElementMaxX && coord.y > 0 && coord.z < LevelGenerator.currentCornerElementMaxZ) {
            //LowerNorthEast
            nearGridElements[4] = LevelGenerator.instance.GetGridElement(coord.x, coord.y - 1, coord.z);
        }

        if (coord.x > LevelGenerator.currentCornerElementMinX && coord.y > 0 & coord.z < LevelGenerator.currentCornerElementMaxZ) {
            //LowerNorthWest
            nearGridElements[5] = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y - 1, coord.z);
        }

        if (coord.x > LevelGenerator.currentCornerElementMinX && coord.y > 0 & coord.z > LevelGenerator.currentCornerElementMinZ) {
            //LowerSouthWest
            nearGridElements[6] = LevelGenerator.instance.GetGridElement(coord.x - 1, coord.y - 1, coord.z - 1);
        }

        if (coord.x < LevelGenerator.currentCornerElementMaxX && coord.y > 0 && coord.z > LevelGenerator.currentCornerElementMinZ) {
            //LowerSouthEast
            nearGridElements[7] = LevelGenerator.instance.GetGridElement(coord.x, coord.y - 1, coord.z - 1);
        }
        SetCornerElement();
    }
}
