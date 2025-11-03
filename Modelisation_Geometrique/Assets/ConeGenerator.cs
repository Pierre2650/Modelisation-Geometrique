using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ConeGenerator : MonoBehaviour
{
    private MeshFilter myMF;

    [Header("Cones")]
    public int hauteurCn;
    public float rayonCn;
    public int nbDroites;
    private Vector3 start;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        myMF = GetComponent<MeshFilter>();
        myMF.mesh.Clear();
        start = transform.position;
    }

    public void generateCone()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector3> P = new List<Vector3>();
        // =========  Points  =============


        Vector3 sommet = new Vector3(start.x, start.y, start.z + hauteurCn);
        for (int i = 0; i < nbDroites; i++)
        {

            Vector3 posP = new Vector3(start.x + rayonCn * Mathf.Cos((2 * Mathf.PI * i) / nbDroites), start.y + rayonCn * Mathf.Sin((2 * Mathf.PI * i) / nbDroites), start.z);
            P.Add(posP);



        }

        //====== Quads ==============
        vertices.Add(start);//0
        vertices.Add(sommet); // 1

        int nbVertices = 2;
        for (int i = 0; i < nbDroites - 1; i++, nbVertices += 2)
        {

            vertices.Add(P[i]); //nbVertices
            vertices.Add(P[i + 1]); //nbVertices+1


            //Generate Triagles droites
            triangles.Add(nbVertices);
            triangles.Add(nbVertices + 1);
            triangles.Add(1);

            //Genetare Quads Limites
            triangles.Add(nbVertices + 1);
            triangles.Add(nbVertices);
            triangles.Add(0);



        }

        //====== Last Triagle ==============
        triangles.Add(nbVertices - 1);
        triangles.Add(2);
        triangles.Add(1);

        // Genetare Triangles Limites
        triangles.Add(2);
        triangles.Add(nbVertices - 1);
        triangles.Add(0);

        myMF.mesh.vertices = vertices.ToArray();
        myMF.mesh.triangles = triangles.ToArray();
    }

    public void resetMesh()
    {
        myMF.mesh.Clear();
    }
}
