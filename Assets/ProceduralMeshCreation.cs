using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]

public class ProceduralMeshCreation : MonoBehaviour
{
    private Mesh p_mesh;
    private Vector3[] p_vertices;
    private int[] p_triangles;
    private Color32[] p_colors;
    private Vector3[] p_normals;

    // Start is called before the first frame update
    void Start()
    {
        p_mesh = new Mesh();

        p_vertices = new Vector3[4];
        p_vertices[0] = new Vector3(0, 0, 0);
        p_vertices[1] = new Vector3(0, 1, 0);
        p_vertices[2] = new Vector3(1, 1, 0);

        p_triangles = new int[] { 0, 1, 2, 0, 2, 1 };

        p_normals = new Vector3[4];
        Vector3 V1 = p_vertices[p_triangles[1]] - p_vertices[p_triangles[0]];
        Vector3 V2 = p_vertices[p_triangles[2]] - p_vertices[p_triangles[0]];
        Vector3 N = Vector3.Cross(V1, V2).normalized;
        p_normals[p_triangles[0]] = N;
        p_normals[p_triangles[1]] = N;
        p_normals[p_triangles[2]] = N;

        p_colors = new Color32[4];
        p_colors[0] = Color.red;
        p_colors[1] = Color.green;
        p_colors[2] = Color.blue;

        p_mesh.Clear();
        p_mesh.vertices = p_vertices;
        p_mesh.triangles = p_triangles;
        p_mesh.colors32 = p_colors;
        p_mesh.normals = p_normals;

        GetComponent<MeshFilter>().mesh = p_mesh;
        DebugNormals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void DebugNormals()
    {
        for (int num_vert = 0; num_vert < p_vertices.Length; num_vert++)
            Debug.DrawRay(transform.position + p_vertices[num_vert], p_normals[num_vert],
            Color.red, 30, false);
    }
}
