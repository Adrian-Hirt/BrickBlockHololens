using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerMeshes : MonoBehaviour
{
    public static CornerMeshes instance;
    private Dictionary<string, Mesh> meshes;
    public GameObject mesh;
    void Awake()
    {
        instance = this;
        meshes = new Dictionary<string, Mesh>();
        Initialize();
    }

    private void Initialize()
    {
        foreach (Transform child in mesh.transform)
        {
            meshes.Add(child.name, child.GetComponent<MeshFilter>().sharedMesh);
        }
    }

    public Mesh GetCornerMesh(int bitmask, int level)
    {
        Mesh res;
        if (level > 1)
        {
            if (meshes.TryGetValue(bitmask.ToString(), out res))
            {
                return res;
            }
        }
        else if (level == 0)
        {
            if (meshes.TryGetValue(0 + "_" + bitmask.ToString(), out res))
            {
                return res;
            }

        }
        else if (level == 1)
        {
            if (meshes.TryGetValue(1 + "_" + bitmask.ToString(), out res))
            {
                return res;
            }

        }
        else if (level == LevelGenerator.instance.height)
        {
            if (meshes.TryGetValue(2 + "_" + bitmask.ToString(), out res))
            {
                return res;
            }
        }

        return null;
    }
}
