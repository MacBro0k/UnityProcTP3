using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTerrain : MonoBehaviour
{
    private Mesh p_mesh;
    private Vector3[] p_vertices;
    private int[] p_triangles;

    public int dimension = 160; // Dimension du terrain
    public int resolution = 8; // Résolution du maillage plan (puissance de 2)

    void Start()
    {
        // Création du maillage
        p_mesh = new Mesh();
        p_mesh.Clear();
        p_mesh.name = "Terrain";

        // Création des sommets
        p_vertices = new Vector3[resolution * resolution];
        float spacing = (float)dimension / (resolution - 1);
        float offset = dimension / 2f;
        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                p_vertices[z * resolution + x] = new Vector3(x * spacing - offset, 0, z * spacing - offset);
            }
        }


        // Création des triangles
        p_triangles = new int[(2 * (resolution - 1) * (resolution - 1)) * 3];
        int t = 0;
        for (int z = 0; z < resolution - 1; z++)
        {
            for (int x = 0; x < resolution - 1; x++)
            {
                int i = z * resolution + x;
                p_triangles[t++] = i;
                p_triangles[t++] = i + resolution;
                p_triangles[t++] = i + resolution + 1;

                p_triangles[t++] = i;
                p_triangles[t++] = i + resolution + 1;
                p_triangles[t++] = i + 1;
            }
        }
        p_mesh.vertices = p_vertices;
        p_mesh.triangles = p_triangles;

        // Recalculer les normales pour l'éclairage
        p_mesh.RecalculateNormals();

        // Assigner le maillage à l'objet
        GetComponent<MeshFilter>().mesh = p_mesh;
    }
}