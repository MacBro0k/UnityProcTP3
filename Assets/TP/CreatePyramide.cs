using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CreatePyramide : MonoBehaviour
{
    public float c;
    public float h;
    private Vector3[] p_vertices;
    private Vector3[] p_normals;
    private int[] p_triangles;
    private Mesh p_mesh;
    private Color32[] p_colors;
    // Use this for initialization
    void Start()
    {
        float cc = c / 2.0f;
        p_mesh = new Mesh();
        p_mesh.Clear();
        p_mesh.name = "MyMeshPyramide";
        p_vertices = new Vector3[5];
        // points de la base
        p_vertices[0] = new Vector3(-cc, 0, -cc);
        p_vertices[1] = new Vector3(-cc, 0, +cc);
        p_vertices[2] = new Vector3(+cc, 0, +cc);
        p_vertices[3] = new Vector3(+cc, 0, -cc);
        p_vertices[4] = new Vector3(0, h, 0);

        p_mesh.vertices = p_vertices;

        p_colors = new Color32[5];
        p_colors[0] = Color.red;
        p_colors[1] = Color.green;
        p_colors[2] = Color.blue;
        p_colors[3] = Color.cyan;
        p_colors[4] = Color.white;
        p_mesh.colors32 = p_colors;

        p_triangles = new int[] { 0, 1, 2,  0, 2, 3,  0, 1, 4,   1, 2, 4,   2, 3, 4,    4, 3, 0};
        p_mesh.triangles = p_triangles;
        p_normals = new Vector3[5];
        // normales des triangles de la base
        p_normals[0] = p_normals[1] = p_normals[2] = p_normals[3] = new Vector3(0, -1, 0);
        // normales des autres triangles
        //Vector3 N;
        //for (int num_triangle = 2; num_triangle <= 5; num_triangle++)
        //{
        //    N = NormalTriangle(num_triangle);
        //    p_normals[p_triangles[num_triangle * 3 + 0]] = N;
        //    p_normals[p_triangles[num_triangle * 3 + 1]] = N;
        //    p_normals[p_triangles[num_triangle * 3 + 2]] = N;
        //}
        p_mesh.normals = p_normals;
                GetComponent<MeshFilter>().mesh = p_mesh;
    }

    // Calcule la normale (normalis�e) au triangle numTri
    //Vector3 NormalTriangle(int numTri)
    //{
    //    Vector3 normal = new Vector3(0, 0, 0);

    //    for (int i = 0; i < numTri; i++)
    //    int v1 = p_vertices[i] - 
    //}
}