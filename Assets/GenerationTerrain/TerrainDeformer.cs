using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TerrainDeformer : MonoBehaviour
{
    private CreateTerrain createTerrain;
    private Mesh p_mesh;
    private Vector3[] p_vertices;
    private List<int>[] p_vertexNeighbors; // Liste des indices des vertices voisins pour chaque vertex

    public float radius = 1.0f; // Rayon du pattern de déformation
    public AnimationCurve attenuationCurve; // Courbe d'atténuation du pattern
    public float intensity = 1.0f; // Intensité de la déformation

    private const float MAX_RADIUS = 10.0f;
    private const float MIN_RADIUS = 0.1f;
    private const float RADIUS_STEP = 0.1f;

    void Start()
    {
        createTerrain = GetComponent<CreateTerrain>();
        p_mesh = createTerrain.p_mesh;
        p_vertices = createTerrain.p_vertices;
        p_vertexNeighbors = createTerrain.p_vertexNeighbors;
    }

    Vector3 FindClosestVertexIndex(Vector3 point)
    {
        int closestVertexIndex = -1;
        float closestDistance = float.MaxValue;

        // Parcourir tous les vertices pour trouver celui qui est le plus proche du point donné
        for (int i = 0; i < p_vertices.Length; i++)
        {
            float distance = Vector3.Distance(p_vertices[i], point);
            Debug.Log("Distance: " + distance);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestVertexIndex = i;
            }
        }

        if (closestVertexIndex == -1)
        {
            Debug.LogError("No closest vertex found!");
        }
        Debug.Log("Closest vertex index: " + closestVertexIndex);
        return p_vertices[closestVertexIndex];
    }

    void DeformTerrain(Vector3 point, bool isDepressing)
    {
        Debug.Log("depressing" + isDepressing);
        // Convertir le point dans l'espace local de l'objet
        point = transform.InverseTransformPoint(point);

        // Parcourir tous les vertices pour déterminer ceux qui sont affectés par le pattern de déformation
        for (int i = 0; i < p_vertices.Length; i++)
        {
            Vector3 vertex = p_vertices[i];
            float distance = Vector3.Distance(vertex, point);

            // Vérifier si le vertex est dans le rayon du pattern de déformation
            if (distance <= radius)
            {
                // Calculer la force de la déformation en fonction de la distance sur la courbe d'atténuation
                float attenuation = attenuationCurve.Evaluate(distance / radius);

                // Modifier la hauteur du vertex en fonction de la force de la déformation et de l'intensité
                float height = vertex.y + (attenuation * intensity * (isDepressing ? -1 : 1));
                p_vertices[i] = new Vector3(vertex.x, height, vertex.z);

                // Modifier la hauteur des vertices voisins également
                foreach (int neighborIndex in p_vertexNeighbors[i])
                {
                    Vector3 neighbor = p_vertices[neighborIndex];
                    float neighborDistance = Vector3.Distance(neighbor, point);
                    if (neighborDistance <= radius)
                    {
                        float neighborAttenuation = attenuationCurve.Evaluate(neighborDistance / radius);
                        float neighborHeight = neighbor.y + (neighborAttenuation * intensity * (isDepressing ? -1 : 1));
                        p_vertices[neighborIndex] = new Vector3(neighbor.x, neighborHeight, neighbor.z);
                    }
                }
            }
        }

        // Mettre à jour le maillage avec les nouvelles hauteurs des vertices
        p_mesh.vertices = p_vertices;
        p_mesh.RecalculateNormals();

        // Mettre à jour le MeshCollider pour le raycast
        MeshCollider meshCollider = createTerrain.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = p_mesh;
    }

    void Update()
    {
        // Détection du clic gauche en maintenant CTRL enfoncé
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(0) && ! EventSystem.current.IsPointerOverGameObject())
        {
            // Lancer un raycast depuis la caméra
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                DeformTerrain(FindClosestVertexIndex(hitInfo.point), true);
            }
        }
        else if (Input.GetMouseButton(0) && ! EventSystem.current.IsPointerOverGameObject())
        {
            // Lancer un raycast depuis la caméra
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo)){
                // Appeler la méthode de déformation du terrain dans le script TerrainDeformer
                DeformTerrain(FindClosestVertexIndex(hitInfo.point), false);
            }   
        } 
        if (Input.GetKey(KeyCode.LeftShift)){
            // Détection de la molette de la souris
            if (Input.mouseScrollDelta.y != 0)
            {
                Debug.Log("Scroll: " + Input.mouseScrollDelta.y);
                // Modifier le rayon du pattern en fonction de la direction de la molette
                float delta = Input.mouseScrollDelta.y > 0 ? RADIUS_STEP : -RADIUS_STEP;
                radius += delta;

                // Limiter le rayon dans la plage autorisée
                radius = Mathf.Clamp(radius, MIN_RADIUS, MAX_RADIUS);
                Debug.Log("Radius: " + radius);
            }
        }
    }
}
