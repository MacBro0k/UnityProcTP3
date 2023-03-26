using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTerrain : MonoBehaviour
{
    public Mesh p_mesh;
    public Vector3[] p_vertices;
    private int[] p_triangles;
    public List<int>[] p_vertexNeighbors; // Liste des indices des vertices voisins pour chaque vertex


    public int dimension = 160; // Dimension du terrain
    public int resolution = 8; // Résolution du maillage plan (puissance de 2)
    
    // Enum pour les types d'affichage du gizmo
    [System.Flags]
    public enum VisualizationMode
    {
        None = 0,
        Vertices = 1,
        Edges = 2,
        Normals = 4,
    }

    public VisualizationMode visualizationMode = VisualizationMode.None;

    void Awake()
    {
        // Création du maillage
        p_mesh = new Mesh();
        p_mesh.Clear();
        p_mesh.name = "Terrain";

        // Ajouter un MeshCollider au terrain
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = p_mesh;

        // Création des sommets
        p_vertices = new Vector3[resolution * resolution];
        p_vertexNeighbors = new List<int>[p_vertices.Length]; // Initialisation de la liste des vertices voisins
        float spacing = (float)dimension / (resolution - 1);
        float offset = dimension / 2f;
        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                p_vertices[z * resolution + x] = new Vector3(x * spacing - offset, 0, z * spacing - offset);
                p_vertexNeighbors[z * resolution + x] = new List<int>(); // Initialisation de la liste des vertices voisins pour chaque vertex
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

                // Ajout des indices des vertices voisins pour chaque vertex
                p_vertexNeighbors[i].Add(i + 1);
                p_vertexNeighbors[i].Add(i + resolution);
                p_vertexNeighbors[i + 1].Add(i);
                p_vertexNeighbors[i + resolution].Add(i);
                p_vertexNeighbors[i + resolution + 1].Add(i + resolution);
                p_vertexNeighbors[i + resolution + 1].Add(i + resolution + 1);
            }
        }
        p_mesh.vertices = p_vertices;
        p_mesh.triangles = p_triangles;

        // Recalculer les normales pour l'éclairag
        p_mesh.RecalculateNormals();

        // Assigner le maillage à l'objet
        GetComponent<MeshFilter>().mesh = p_mesh;
    }

    void OnDrawGizmos()
    {
        if (visualizationMode == VisualizationMode.None)
            return;

        // Affichage des sommets
        if ((visualizationMode & VisualizationMode.Vertices) != 0)
        {
            Gizmos.color = new Color(1.0f, 0.5f, 0.0f);
            foreach (Vector3 vertex in p_mesh.vertices)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vertex), 0.8f);
            }
        }

        // Affichage des arêtes
        if ((visualizationMode & VisualizationMode.Edges) != 0)
        {
            if (p_mesh == null || visualizationMode == VisualizationMode.None)
                return;
            Gizmos.color = Color.white;
            for (int i = 0; i < p_mesh.triangles.Length; i += 3)
            {
                Gizmos.DrawLine(
                    transform.TransformPoint(p_mesh.vertices[p_mesh.triangles[i]]),
                    transform.TransformPoint(p_mesh.vertices[p_mesh.triangles[i + 1]])
                );
                Gizmos.DrawLine(
                    transform.TransformPoint(p_mesh.vertices[p_mesh.triangles[i + 1]]),
                    transform.TransformPoint(p_mesh.vertices[p_mesh.triangles[i + 2]])
                );
                Gizmos.DrawLine(
                    transform.TransformPoint(p_mesh.vertices[p_mesh.triangles[i + 2]]),
                    transform.TransformPoint(p_mesh.vertices[p_mesh.triangles[i]])
                );
            }
        }

        // Affichage des normales
        if ((visualizationMode & VisualizationMode.Normals) != 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < p_mesh.triangles.Length; i += 3)
            {
                Vector3 p1 = p_mesh.vertices[p_mesh.triangles[i]];
                Vector3 p2 = p_mesh.vertices[p_mesh.triangles[i + 1]];
                Vector3 p3 = p_mesh.vertices[p_mesh.triangles[i + 2]];
                Vector3 normal = (p_mesh.normals[p_mesh.triangles[i]] + p_mesh.normals[p_mesh.triangles[i + 1]] + p_mesh.normals[p_mesh.triangles[i + 2]]) / 3f; // moyenne des normales des sommets
                Vector3 center = (p1 + p2 + p3) / 3f;
                Gizmos.DrawLine(transform.TransformPoint(center), transform.TransformPoint(center + normal));
            }
        }
    }
}

