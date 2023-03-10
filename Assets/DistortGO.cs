using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DistortGO : MonoBehaviour
{
    public GameObject PickObj; // le GameObject qui sera placé en chaque sommet
    private Mesh p_mesh;
    private Vector3[] p_vertices;
    private Dictionary<int, List<int>> les_po;
    void Start()
    {
        p_mesh = gameObject.GetComponent<MeshFilter>().mesh;
        p_vertices = p_mesh.vertices;
        const float SEUIL_DISTANCE_VERTICES_SIMILAIRES = 0.01f;
        bool[] bool_vert = new bool[p_vertices.Length];
        // indique si un vertex a été traité / faux par defaut
        for (int i = 0; i < bool_vert.Length; i++) bool_vert[i] = false;
        // l'ensemble des PickingObjects , chacun a un identifiant unique GetInstanceID
        // à chaque PickingObjetc est associé une liste de vertices « similaires »
        // un vertex est jugé similaire d'un autre s'il se trouve à une
        // distance négligeable, i.e. inférieure à un seuil
        // la structure de données les_po permet d’associer un nom à une liste de vertices
        les_po = new Dictionary<int, List<int>>();
        int index_vert = 0;
        int nb_po = 0;
        while (index_vert < p_vertices.Length) // traiter tous les vertices
        {
            if (!bool_vert[index_vert])
            {
                bool_vert[index_vert] = true;
                GameObject po = Instantiate(PickObj);
                po.name = "po" + nb_po;
                nb_po++;
                po.transform.position = transform.position + p_vertices[index_vert];
                // idem ci dessous
                po.transform.position = transform.TransformPoint(p_vertices[index_vert]);
                // TransformPoint converts the vertex's local position into world space.
                les_po.Add(po.GetInstanceID(), new List<int> { index_vert });
                // à faire !!
                // Vérifier quels sont les vertices similaires
                // les associer à cet objet po dans le dictionnaire les_po
                for (int i = index_vert+1; i < p_vertices.Length; i++) {
                    if (Vector3.Distance(p_vertices[i], p_vertices[index_vert]) < SEUIL_DISTANCE_VERTICES_SIMILAIRES && bool_vert[i] != true)
                    {
                        les_po[po.GetInstanceID()].Add(i);
                        bool_vert[i] = true;
                    }
                }
            }
            index_vert++;
        }
    }

    public void MoveVertex(GameObject pickedOBJ)
    {
        //Vector3[] vertices = les_po[pickedOBJ.GetInstanceID()][];
        //p_vertices[].
    }
}