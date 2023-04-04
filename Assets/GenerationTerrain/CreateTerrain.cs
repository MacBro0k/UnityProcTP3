using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTerrain : MonoBehaviour
{
    [HideInInspector]
    public Mesh p_mesh;

    [HideInInspector]
    public Vector3[] p_vertices;

    [HideInInspector]
    public List<int>[] p_vertexNeighbors; // Liste des indices des vertices voisins pour chaque vertex

    private int[] p_triangles;


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

    //section de géneration de terrain de l'inspector
    [Header("Terrain Generation")]

    public Texture2D heightmap;
    public float minHeight = 0.0f;
    public float maxHeight = 10.0f;

    void Awake()
    {
        // Création du maillage
        p_mesh = new Mesh();
        p_mesh.Clear();
        p_mesh.name = "Terrain";

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

        // Ajouter un MeshCollider au terrain
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = p_mesh;
    }

    // Rééchantillonne l'image 'src' pour qu'elle ait la taille 'dstWidth' x 'dstHeight'
    public static Texture2D ResizeTexture(Texture2D src, int dstWidth, int dstHeight)
    {
        Color[] srcPixels = src.GetPixels();
        Color[] dstPixels = new Color[dstWidth * dstHeight];
        float srcAspect = (float)src.width / (float)src.height;
        float dstAspect = (float)dstWidth / (float)dstHeight;

        for (int y = 0; y < dstHeight; y++)
        {
            for (int x = 0; x < dstWidth; x++)
            {
                float u = ((float)x + 0.5f) / (float)dstWidth;
                float v = ((float)y + 0.5f) / (float)dstHeight;

                float px = u * src.width;
                float py = v * src.height;

                int ix0 = Mathf.FloorToInt(px - 0.5f);
                int iy0 = Mathf.FloorToInt(py - 0.5f);
                int ix1 = ix0 + 1;
                int iy1 = iy0 + 1;

                float tx = px - (float)ix0 - 0.5f;
                float ty = py - (float)iy0 - 0.5f;

                float w00 = (1.0f - tx) * (1.0f - ty);
                float w01 = tx * (1.0f - ty);
                float w10 = (1.0f - tx) * ty;
                float w11 = tx * ty;

                ix0 = Mathf.Clamp(ix0, 0, src.width - 1);
                iy0 = Mathf.Clamp(iy0, 0, src.height - 1);
                ix1 = Mathf.Clamp(ix1, 0, src.width - 1);
                iy1 = Mathf.Clamp(iy1, 0, src.height - 1);

                Color c00 = srcPixels[iy0 * src.width + ix0];
                Color c01 = srcPixels[iy0 * src.width + ix1];
                Color c10 = srcPixels[iy1 * src.width + ix0];
                Color c11 = srcPixels[iy1 * src.width + ix1];

                Color dstColor = w00 * c00 + w01 * c01 + w10 * c10 + w11 * c11;
                dstPixels[y * dstWidth + x] = dstColor;
            }
        }

        Texture2D dst = new Texture2D(dstWidth, dstHeight);
        dst.SetPixels(dstPixels);
        dst.Apply();

        return dst;
    }


    // Bouton pour générer le terrain à partir d'une heightmap
    public void GenerateTerrain()
    {
        if(heightmap == null)
        {
            Debug.LogError("Heightmap is null");
            return;
        }
        // Vérifier que la résolution de l'image ne dépasse pas le nombre maximum de vertices
        int heightmapResolution = Mathf.Min(heightmap.width, heightmap.height); // On prend la plus petite dimension

        // Calculer la résolution du terrain actuel
        int terrainResolution = p_vertices.Length;

        // Adapter la taille de la heightmap si nécessaire
        if (heightmapResolution != resolution*resolution)
        {
            // Créer une nouvelle texture avec la résolution du terrain
            Texture2D resizedHeightmap = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false);
            for (int z = 0; z < resolution; z++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    // Interpoler la valeur de pixel correspondant au nouveau vertex depuis la heightmap originale
                    float u = x / (float)resolution;
                    float v = z / (float)resolution;
                    resizedHeightmap.SetPixel(x, z, heightmap.GetPixelBilinear(u, v));
                }
            }
            resizedHeightmap.Apply();
            heightmap = resizedHeightmap;
        }

        // Modifier les vertices
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                float height = Mathf.Lerp(minHeight, maxHeight, heightmap.GetPixel(i, j).grayscale);
                p_vertices[i * resolution + j].y += height;
            }
        }

        // Assigner les vertices et recalculer les normales
        p_mesh.vertices = p_vertices;
        p_mesh.RecalculateNormals();

        // Assigner le maillage au MeshCollider
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = p_mesh;
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

