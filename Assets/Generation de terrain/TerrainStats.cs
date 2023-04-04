using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerrainStats : MonoBehaviour
{
    public TextMeshProUGUI NbVertex;
    public TextMeshProUGUI NbTriangle;

    private Mesh Terrain;

    private string NbVertexLabel = "Nombre de vertex : ";
    private string NbTriangleLabel = "Nombre de triangle : ";
    // Start is called before the first frame update
    void Start()
    {
        Terrain = gameObject.GetComponent<CreateTerrain>().p_mesh;
        
        NbVertex.SetText(NbVertexLabel + Terrain.vertices.Length);
        NbTriangle.SetText(NbTriangleLabel + Terrain.triangles.Length);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
