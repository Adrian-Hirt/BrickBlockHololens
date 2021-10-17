using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGenerator : MonoBehaviour {
    int width = 5;
    int height = 10;

    public GridElement gridElement;
    public GridElement[] gridElements;


    // Start is called before the first frame update
    void Start() {
        gridElements = new GridElement[width * width * height];

        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {
                for(int z = 0; z < width; z++) {
                    GridElement gridElementInstance = Instantiate(gridElement, new Vector3(x, y, z), Quaternion.identity, this.transform);
                    gridElementInstance.name = "GridElement_" + x + "_" + y + "_" + z;
                    gridElements[x + width * (z + width * y)] = gridElementInstance;
                }
            }
        }
    }
}
