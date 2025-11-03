using System.Collections.Generic;
using UnityEngine;

public class CylinderGenerator : MonoBehaviour
{
    private MeshFilter myMF;

    [Header("Cylinder")]
    public int hauteurCyl;
    public int rayonCyl;
    public int nbMeridientsCyl;
    private Vector3 start;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myMF = GetComponent<MeshFilter>();
        myMF.mesh.Clear();
        start = Vector3.zero;
    }

    public void generateCylinder()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        List<Vector3> PPrime = new List<Vector3>();
        List<Vector3> P = new List<Vector3>();

        //vertices.Add(new Vector3(start.x , start.y  ,  -hauteur / 2));
        for (int i = 0; i < nbMeridientsCyl; i++)
        {

            Vector3 posTest = new Vector3(start.x + rayonCyl * Mathf.Cos((2 * Mathf.PI * i) / nbMeridientsCyl), start.y + rayonCyl * Mathf.Sin((2 * Mathf.PI * i) / nbMeridientsCyl), start.z  -hauteurCyl / 2);
            PPrime.Add(posTest);

            Vector3 posTest2 = new Vector3(start.x + rayonCyl * Mathf.Cos((2 * Mathf.PI * i) / nbMeridientsCyl), start.y + rayonCyl * Mathf.Sin((2 * Mathf.PI * i) / nbMeridientsCyl), start.z + hauteurCyl / 2);
            P.Add(posTest2);

        }

        int j = 0;
        for (int i = 0; i < nbMeridientsCyl - 1; i++, j += 6)
        {

            vertices.Add(P[i]);
            vertices.Add(PPrime[i]);

            vertices.Add(PPrime[i + 1]);
            vertices.Add(P[i + 1]);
            vertices.Add(new Vector3(start.x, start.y, -hauteurCyl / 2));
            vertices.Add(new Vector3(start.x, start.y, hauteurCyl / 2));


            //Generate rectangles Meridieans
            triangles.Add(j);
            triangles.Add(j + 1);
            triangles.Add(j + 2);

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

        triangles.Add(j - 3);
        triangles.Add(j - 4);
        triangles.Add(1);

        triangles.Add(j - 3);
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

    public void resetMesh()
    {
        myMF.mesh.Clear();
    }
}
