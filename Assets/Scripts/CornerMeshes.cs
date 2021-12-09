using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerMeshes : MonoBehaviour
{
    public static CornerMeshes instance;
    private Dictionary<string, Mesh> meshes;
    public GameObject defaultMesh;
    public GameObject simpleMesh;
    
    public enum MeshTypes
    {
        Default,
        Simple
    }
    private MeshTypes currMesh;

    void Awake()
    {
        instance = this;
        meshes = new Dictionary<string, Mesh>();
        currMesh = MeshTypes.Default;
        Initialize();
    }

    private void Initialize()
    {
        foreach (Transform child in defaultMesh.transform)
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
        else if (level == LevelGenerator.height)
        {
            if (meshes.TryGetValue(2 + "_" + bitmask.ToString(), out res))
            {
                return res;
            }
        }

        return null;
    }

    public void UpdateMesh()
    {
        meshes.Clear();

        switch (currMesh)
        {
            case MeshTypes.Default:
                foreach (Transform child in simpleMesh.transform)
                {
                    meshes.Add(child.name, child.GetComponent<MeshFilter>().sharedMesh);
                }
                currMesh = MeshTypes.Simple;
                break;
            case MeshTypes.Simple:
                foreach (Transform child in defaultMesh.transform)
                {
                    meshes.Add(child.name, child.GetComponent<MeshFilter>().sharedMesh);
                }
                currMesh = MeshTypes.Default;
                break;
        }
    }
}
