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
        GenerateTerrain();
    }

    // Generation du terrain selon la resolution et la dimension
    private void GenerateTerrain()
    {
        p_mesh = new Mesh();
        p_mesh.name = "MyProceduralTerrain";
        int margin = Dimension / Resolution;
        // les vertices du terrain
        for (int i = 0; i < Resolution; i++)
        {
            for (int j = 0; j < Resolution; j++)
            {
                int index = i * Resolution + j;
                p_vertices[index] = new Vector3(i, 0, j);
                p_colors[index] = Color.white;
                p_normals[index] = Vector3.up;
            }
        }
        // les triangles du terrain
        for (int i = 0; i < Resolution - 1; i++)
        {
            for (int j = 0; j < Resolution - 1; j++)
            {
                int index = i * (Resolution - 1) + j;
                p_triangles[index * 6] = i * Resolution + j;
                p_triangles[index * 6 + 1] = i * Resolution + j + 1;
                p_triangles[index * 6 + 2] = (i + 1) * Resolution + j;
                p_triangles[index * 6 + 3] = (i + 1) * Resolution + j;
                p_triangles[index * 6 + 4] = i * Resolution + j + 1;
                p_triangles[index * 6 + 5] = (i + 1) * Resolution + j + 1;
            }
        }

        p_mesh.vertices = p_vertices;
        p_mesh.triangles = p_triangles;
        p_mesh.normals = p_normals;
        p_mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = p_mesh;
    }
}