using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCube : MonoBehaviour
{
    public enum TypeCube { Cube24 , Cube8 }
    public TypeCube typeCube;
    public float width = 1.0f;

    private Mesh p_mesh;
    private Vector3[] p_vertices;
    private Vector3[] p_normals;
    private int[] p_triangles;

    private Vector3 p0 = new Vector3(-1, -1, 1);
    private Vector3 p1 = new Vector3(-1, 1, 1);
    private Vector3 p2 = new Vector3(1, 1, 1);
    private Vector3 p3 = new Vector3(1, -1, 1);
    private Vector3 p4 = new Vector3(-1, -1, -1);
    private Vector3 p5 = new Vector3(-1, 1, -1);
    private Vector3 p6 = new Vector3(1, 1, -1);
    private Vector3 p7 = new Vector3(1, -1, -1);

    private void CreerCube8() {
        p_mesh = new Mesh();
        p_mesh.name = "MyProceduralCube8Vertices";
        // les 8 vertices partagés du cube
        p_vertices = new Vector3[8] {
            p0,p1,p2,p3,
            p4,p5,p6,p7
        };
        p_normals = new Vector3[p_vertices.Length];
        // les indices des 3 vertices des 12 triangles (2 pour chaque face du cube)
        p_triangles = new int[12*3] {
            0,2,1, 0,3,2, // devant
            0,1,5, 0,5,4, // gauche
            3,6,2, 3,7,6, // Droite
            7,5,6, 7,4,5, // Derrière
            1,6,5, 1,2,6, // Dessus
            4,3,0, 4,7,3  // dessous
        };
        

        for (int i = 0; i < p_vertices.Length; i++)
        {
            p_normals[i] = p_vertices[i].normalized;
        }
        p_mesh.vertices = p_vertices;
        p_mesh.triangles = p_triangles;
        p_mesh.normals = p_normals;
        p_mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = p_mesh;
    }

    private void CreerCube24() {
        p_mesh = new Mesh();
        p_mesh.name = "MyProceduralCube24Vertices";
        // les 24 vertices non partagés du cube
        p_vertices = new Vector3[]{
            p0,p1,p2,p3,  // devant
            p4,p5,p1,p0,  // gauche
            p3,p2,p6,p7,  // Droite
            p7,p6,p5,p4,  // Derrière
            p1,p5,p6,p2,  // Dessus
            p4,p0,p3,p7   // dessous
        };
        p_normals = new Vector3[p_vertices.Length];
        // les indices des 3 vertices des 12 triangles (2 pour chaque face du cube)
        p_triangles = new int[12*3];
        int index = 0;
        for (int i =0; i<6; i++)   // 6 faces à 2 triangles
        {   
            // triangle 1
            p_triangles[index++] = i * 4;
            p_triangles[index+1] = i * 4 + 1;
            p_triangles[index++] = i * 4 + 3;
            index++;

            // triangle 2
            p_triangles[index++] = i * 4 + 1;
            p_triangles[index+1] = i * 4 + 2;
            p_triangles[index++] = i * 4 + 3;
            index++;
        }
        // à faire :
        //calcul des normales : initialiser un remplir un tableau de 12x3 normales, une pour chaque vertex
        // Associer ces structures p_normals et p_vertices à un maillage et associer le maillage à l’objet
        for (int i = 0; i < p_vertices.Length; i++)
        {
            p_normals[i] = p_vertices[i].normalized;
        }
        p_mesh.vertices = p_vertices;
        p_mesh.triangles = p_triangles;
        p_mesh.normals = p_normals;
        p_mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = p_mesh;
    }

    void Start()
    {
        float w = -width / 2.0f;
        float W = width / 2.0f;
        switch (typeCube)
        {
            case TypeCube.Cube8:
                CreerCube8(); break;
            case TypeCube.Cube24:
                CreerCube24(); break;
        }
    }

    void Update()
    {
        DebugNormals();
    }

    private void DebugNormals() {
        for (int num_vert = 0; num_vert < p_vertices.Length; num_vert++)
            Debug.DrawRay(transform.position+ p_vertices[num_vert], p_normals[num_vert], Color.red,30,false);
    }
}
