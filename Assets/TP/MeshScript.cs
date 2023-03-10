using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter))]

public class MeshScript : MonoBehaviour
{
    private Mesh p_mesh;
    private Vector3[] p_vertices;
    private int[] p_triangles;
    private Color32[] p_couleurs;
    private Vector3[] p_normales;
    // Start is called before the first frame update
    void Start()
    {
        p_vertices = new Vector3[3];
        p_vertices[0] = new Vector3(0, 0, 0);
        p_vertices[1] = new Vector3(0, 1, 0);
        p_vertices[2] = new Vector3(1, 1, 0);

        p_mesh = new Mesh();
        p_mesh.vertices = p_vertices;

        p_triangles = new int[] {0, 1, 2};
        p_mesh.triangles = p_triangles;

        p_couleurs = new Color32[3];
        p_couleurs[0] = new Color32(255, 0, 0, 0);
        p_couleurs[1] = new Color32(0, 255, 0, 0);
        p_couleurs[2] = new Color32(0, 0, 255, 0);

        p_mesh.colors32 = p_couleurs;

        p_normales = new Vector3[3];
        p_normales[0] = new Vector3(0, 0, -1);
        p_normales[1] = new Vector3(0, 0, -1);
        p_normales[2] = new Vector3(1, 0, 0);

        p_mesh.normals = p_normales;

        Debug.DrawRay(transform.TransformPoint(p_vertices[0]), transform.TransformDirection(p_normales[0]), Color.red, 100);
        Debug.DrawRay(transform.TransformPoint(p_vertices[1]), transform.TransformDirection(p_normales[1]), Color.red, 100);
        Debug.DrawRay(transform.TransformPoint(p_vertices[2]), transform.TransformDirection(p_normales[2]), Color.red, 100);
        Debug.DrawRay(transform.TransformPoint(p_vertices[1]), Vector3.Cross(p_normales[0], p_normales[1]), Color.green, 100);

        GetComponent<MeshFilter>().mesh = p_mesh;
        GetComponent<MeshFilter>().sharedMesh = p_mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
