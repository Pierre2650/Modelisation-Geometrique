
using System.Collections.Generic;
using UnityEngine;

public class TD1 : MonoBehaviour
{
    private MeshFilter myMF;

    [Header("Plane")]
    public int nbLignes = 0, nbColones = 0;

    [Header("Cylinder")]
    public int nbMeridients;
    public int hauteur;
    public int rayon;

    private List<Vector3> draw = new List<Vector3>();
    private List<Vector3> draw2 = new List<Vector3>();

    private Vector3[] verts = new Vector3[]
    {
        new Vector3(0,0, 0),
        new Vector3(0, 1, 0),
        new Vector3(2, 1, 0),

        new Vector3(0, 0, 0),
        new Vector3(2, 1, 0),
        new Vector3(2, 0, 0),

    };

    private int[] tri = new int[]
    {
        0, 1, 2,
        3 , 4, 5
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myMF = GetComponent<MeshFilter>();  
        myMF.mesh.Clear();
        //myMF.mesh.vertices = verts;
        //myMF.mesh.triangles = tri ;
        //generatePlane();
        generateCylinder(Vector3.zero);

    }


    private void generatePlane()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        int triIndex = 6;
        Vector3 startL = Vector3.zero;

        for (int i = 0; i < nbLignes; i++)
        {
            Vector3 startC = startL;
            for (int j = 0; j < nbColones; j++)
            {
                generateRectangle(startC, vertices, triangles, triIndex);
                startC = new Vector3(startC.x + 2, startC.y, startC.z);
                triIndex += 6;
            }

            startL = new Vector3(startL.x, startL.y, startL.z - 1);
        }


        myMF.mesh.vertices = vertices.ToArray();
        myMF.mesh.triangles = triangles.ToArray();
    }

    private void generateRectangle(Vector3 start, List<Vector3> vertices, List<int> triangles, int trianglesIndex)
    {
        vertices.Add(start);
        vertices.Add(new Vector3(start.x, start.y , start.z + 1));
        vertices.Add(new Vector3(start.x + 2, start.y , start.z + 1));

        vertices.Add(start);
        vertices.Add(new Vector3(start.x + 2, start.y, start.z + 1));
        vertices.Add(new Vector3(start.x +2, start.y, start.z));

        int i = triangles.Count ;

        for (; i < trianglesIndex; i++) {
            Debug.Log("i triangles = " + i);
            triangles.Add(i);
        }

        Debug.Log("End");
    }

    private void generateCylinder(Vector3 start)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        List<Vector3> PPrime = new List<Vector3>();
        List<Vector3> P = new List<Vector3>();

        //vertices.Add(new Vector3(start.x , start.y  ,  -hauteur / 2));
        for (int i = 0; i < nbMeridients ; i++) {

            Vector3 posTest = new Vector3(start.x + rayon * Mathf.Cos((2 * Mathf.PI * i) / nbMeridients), start.y + rayon * Mathf.Sin((2 * Mathf.PI * i) / nbMeridients), start.z + -hauteur / 2);
            PPrime.Add(posTest);

            Vector3 posTest2 = new Vector3(start.x + rayon * Mathf.Cos((2 * Mathf.PI * i) / nbMeridients), start.y + rayon * Mathf.Sin((2 * Mathf.PI * i) / nbMeridients), start.z + hauteur / 2);
            P.Add(posTest2);

        }

        int j = 0;
        for (int i = 0 ; i < nbMeridients-1; i++, j+=6)
        {

            vertices.Add(P[i]);
            vertices.Add(PPrime[i]);
            
            vertices.Add(PPrime[i+1]);
            vertices.Add(P[i+1]);
            vertices.Add(new Vector3(start.x, start.y, -hauteur / 2));
            vertices.Add(new Vector3(start.x, start.y, hauteur / 2));


            //Generate rectangles Meridieans
            triangles.Add(j);
            triangles.Add(j+1);
            triangles.Add(j+2);

            triangles.Add(j);
            triangles.Add(j + 2);
            triangles.Add(j + 3);

            //Genetare Plans Limites
            triangles.Add(j + 1);
            triangles.Add(j + 4);
            triangles.Add(j + 2);

            triangles.Add(j);
            triangles.Add(j + 3);
            triangles.Add(j + 5);




        }
        //====== to last => j+3 ==============
        
        triangles.Add(j-3);
        triangles.Add(j - 4);
        triangles.Add(1);

        triangles.Add(j-3);
        triangles.Add(1);
        triangles.Add(0);

        // Genetare Plans Limites
        triangles.Add(j - 4);
        triangles.Add(j - 2);
        triangles.Add(1);

        triangles.Add(j - 3);
        triangles.Add(0);
        triangles.Add(j - 1);


       

        myMF.mesh.vertices = vertices.ToArray();
        myMF.mesh.triangles = triangles.ToArray();


    }

    private void OnDrawGizmos()
    {
        /*// Draw wire sphere outline.
        Gizmos.color = Color.red;
        foreach(Vector3 D in draw)
        {
            Gizmos.DrawWireSphere(D, 0.3f);
        }

        Gizmos.color = Color.blue;
        foreach (Vector3 D in draw2)
        {
            Gizmos.DrawWireSphere(D, 0.3f);
        }

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(draw2[draw2.Count - 1], 0.3f);*/
    }
}
