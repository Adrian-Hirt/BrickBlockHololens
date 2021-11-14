using UnityEngine;

public class coord
{
    public int x, y, z;
    public coord(int setX, int setY, int setZ)
    {
        x = setX;
        y = setY;
        z = setZ;
    }
}

public class GridElement : MonoBehaviour
{
    private coord coord;
    private Collider col;
    private Renderer rend;
    public CornerElement[] corners;
    bool isEnabled;
    private float elementHeight;
    public bool isGroundElement;

    public void Initialize(int setX, int setY, int setZ, float setElementHeight)
    {
        corners = new CornerElement[8];

        coord = new coord(setX, setY, setZ);
        this.name = "GE_" + this.coord.x + "_" + this.coord.y + "_" + this.coord.z;
        this.col = this.GetComponent<Collider>();
        this.rend = this.GetComponent<Renderer>();
        this.elementHeight = setElementHeight;
        this.transform.localScale = new Vector3(1.0f, elementHeight, 1.0f);
        this.isGroundElement = setY == 0;
        //setting corners

        corners[0] = LevelGenerator.instance.GetCornerElement(coord.x, coord.y, coord.z);
        corners[1] = LevelGenerator.instance.GetCornerElement(coord.x + 1, coord.y, coord.z);
        corners[2] = LevelGenerator.instance.GetCornerElement(coord.x, coord.y, coord.z + 1);
        corners[3] = LevelGenerator.instance.GetCornerElement(coord.x + 1, coord.y, coord.z + 1);
        corners[4] = LevelGenerator.instance.GetCornerElement(coord.x, coord.y + 1, coord.z);
        corners[5] = LevelGenerator.instance.GetCornerElement(coord.x + 1, coord.y + 1, coord.z);
        corners[6] = LevelGenerator.instance.GetCornerElement(coord.x, coord.y + 1, coord.z + 1);
        corners[7] = LevelGenerator.instance.GetCornerElement(coord.x + 1, coord.y + 1, coord.z + 1);
    }

    public coord GetCoord()
    {
        return coord;
    }

    public void SetCornerPositions()
    {
        Vector3 position = this.transform.localPosition;
        float x = coord.x;
        float y = coord.y;
        float z = coord.z;
        float f = 0.5F;
        corners[0].SetPosition(x - f, y - f * elementHeight, z - f);
        corners[1].SetPosition(x + f, y - f * elementHeight, z - f);
        corners[2].SetPosition(x - f, y - f * elementHeight, z + f);
        corners[3].SetPosition(x + f, y - f * elementHeight, z + f);
        corners[4].SetPosition(x - f, y + f * elementHeight, z - f);
        corners[5].SetPosition(x + f, y + f * elementHeight, z - f);
        corners[6].SetPosition(x - f, y + f * elementHeight, z + f);
        corners[7].SetPosition(x + f, y + f * elementHeight, z + f);
    }

    public void SetEnabled()
    {
        this.col.enabled = true;
        this.rend.enabled = false;
        this.isEnabled = true;
        foreach (CornerElement ce in corners)
        {
            ce.SetCornerElement();
        }
    }

    public void SetTapEnabled()
    {
        this.SetEnabled();

        if (this.coord.x == LevelGenerator.currentWidthLow)
        {
            LevelGenerator.instance.AddShellInDirectionX(true);
        }
        else if (this.coord.x == LevelGenerator.currentWidthHigh - 1)
        {
            LevelGenerator.instance.AddShellInDirectionX(false);
        }
        else if (this.coord.z == LevelGenerator.currentLengthLow)
        {
            LevelGenerator.instance.AddShellInDirectionZ(true);
        }
        else if (this.coord.z == LevelGenerator.currentLengthHigh - 1)
        {
            LevelGenerator.instance.AddShellInDirectionZ(false);
        }
        else if (this.coord.y == LevelGenerator.currentHeightLow)
        {
            // Do nothing, as we don't build "down" into the floor
        }
        else if (this.coord.y == LevelGenerator.currentHeightHigh - 1)
        {
            LevelGenerator.instance.AddShellInDirectionY();
        }
    }

    public void SetDisabled()
    {
        this.col.enabled = false;
        this.rend.enabled = false;
        this.isEnabled = false;
        foreach (CornerElement ce in corners)
        {
            ce.SetCornerElement();
        }
    }

    public bool GetEnabled()
    {
        return isEnabled;
    }

    public float GetHeight()
    {
        return elementHeight;
    }
}
