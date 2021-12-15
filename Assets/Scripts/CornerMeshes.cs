using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerMeshes : MonoBehaviour
{
    public static CornerMeshes instance;
    private Dictionary<string, Mesh> defaultMeshes;
    private Dictionary<string, Mesh> simpleMeshes;
    public GameObject defaultMesh;
    public GameObject simpleMesh;
    
    public enum MeshTypes
    {
        Default,
        Simple
    }
    private MeshTypes currMeshType;

    void Awake()
    {
        instance = this;
        defaultMeshes = new Dictionary<string, Mesh>();
        simpleMeshes  = new Dictionary<string, Mesh>();
        currMeshType = MeshTypes.Default;
        Initialize();
    }

    private void Initialize()
    {
        foreach (Transform child in defaultMesh.transform)
        {
            defaultMeshes.Add(child.name, child.GetComponent<MeshFilter>().sharedMesh);
        }
        
        foreach (Transform child in simpleMesh.transform)
        {
            simpleMeshes.Add(child.name, child.GetComponent<MeshFilter>().sharedMesh);
        }
    }

    public Mesh GetCornerMesh(int bitmask, int level)
    {
        switch (currMeshType)
        {
            case MeshTypes.Default:
                return getCornerMesh(bitmask, level, defaultMeshes);
            case MeshTypes.Simple:
                return getCornerMesh(bitmask, level, simpleMeshes);
            default:
                return null;
        }
    }

    private Mesh getCornerMesh(int bitmask, int level, Dictionary<string, Mesh> meshes)
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
        currMeshType = (currMeshType == MeshTypes.Default) ? MeshTypes.Simple : MeshTypes.Default;

        foreach (KeyValuePair<string, CornerElement> entry in LevelGenerator.instance.cornerElementsDict)
        {
            entry.Value.SetCornerElement();
        }
    }
}
