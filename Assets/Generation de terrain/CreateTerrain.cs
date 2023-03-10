using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTerrain : MonoBehaviour
{
    private Mesh p_mesh;
    private Vector3[] p_vertices;
    private int[] p_triangles;
    private Color32[] p_colors;
    private Vector3[] p_normals;
    public int Dimension;
    public int Resolution;
    public bool CentrerPivot;

    // Start is called before the first frame update
    void Awake()
    {
        if(Resolution%2 == 1)
        {
            Debug.LogError("Resolution must be an power of 2");
        }
    }

    void Start()
    {
        GenereateTerrain();
    }

    private void GenereateTerrain()
    {
        p_mesh = new Mesh();
        p_mesh.name = "MyProceduralTerrain";
        GetComponent<MeshFilter>().mesh = p_mesh;
    }
}
