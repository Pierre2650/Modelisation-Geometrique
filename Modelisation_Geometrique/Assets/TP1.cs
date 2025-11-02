
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.EventSystems.EventTrigger;

public class TP1 : MonoBehaviour
{
    private MeshFilter myMF;

    [Header("Plane")]
    public int nbLignes = 0, nbColones = 0;

    [Header("Cylinder")]
    public int nbMeridientsCyl;
    public int hauteurCyl;
    public int rayonCyl;

    [Header("Sphere")]
    public int rayonSph;
    public int nbParallelesSph;
    public int nbMeridientsSph;

    private List<Vector3> draw = new List<Vector3>();
    private Vector3 N = Vector3.zero, S = Vector3.zero;
    //private List<Vector3> draw2 = new List<Vector3>();

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
        //generateCylinder(Vector3.zero);
        generateSphere(Vector3.zero);

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
        for (int i = 0; i < nbMeridientsCyl ; i++) {

            Vector3 posPP = new Vector3(start.x + rayonCyl * Mathf.Cos((2 * Mathf.PI * i) / nbMeridientsCyl), start.y + rayonCyl * Mathf.Sin((2 * Mathf.PI * i) / nbMeridientsCyl), start.z + -hauteurCyl / 2);
            PPrime.Add(posPP);

            Vector3 posP = new Vector3(start.x + rayonCyl * Mathf.Cos((2 * Mathf.PI * i) / nbMeridientsCyl), start.y + rayonCyl * Mathf.Sin((2 * Mathf.PI * i) / nbMeridientsCyl), start.z + hauteurCyl / 2);
            P.Add(posP);

        }

        int j = 0;
        for (int i = 0 ; i < nbMeridientsCyl-1; i++, j+=6)
        {

            vertices.Add(P[i]); //j
            vertices.Add(PPrime[i]); //j+1
            vertices.Add(PPrime[i+1]); //j+2
            vertices.Add(P[i+1]); //j+3

            vertices.Add(new Vector3(start.x, start.y, -hauteurCyl / 2));
            vertices.Add(new Vector3(start.x, start.y, hauteurCyl / 2));


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

        //====== last ==============
        
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

    private void generateSphere(Vector3 centre)
    {

        if (nbMeridientsSph < 3 || nbParallelesSph < 2) { Debug.LogError("Nombre de meridiens  < 3 ou Nombre de paralleles < 2"); return; }

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();


        List<Vector3>[] Parallels = new List<Vector3>[nbParallelesSph-1];

        N = new Vector3(centre.x,centre.y, centre.z + rayonSph);
        S = new Vector3(centre.x, centre.y, centre.z - rayonSph);


        //StartCoroutine(vizualiser(centre,vertices, triangles, Parallels));
        //return;

       for (int i = 1; i < nbParallelesSph; i++)  // from north pole to south pole
        {
            float phi = Mathf.PI * i / nbParallelesSph;  // goes from 0 to π

            List<Vector3> parallelsPts = new List<Vector3>();

            for (int j = 0; j < nbMeridientsSph; j++)  // around the sphere
            {
                float theta = 2 * Mathf.PI * j / nbMeridientsSph; // goes from 0 to 2π

                Vector3 point  = new Vector3(centre.x + rayonSph * Mathf.Sin(phi) * Mathf.Cos(theta), centre.y + rayonSph * Mathf.Sin(phi) * Mathf.Sin(theta) , centre.z + rayonSph * Mathf.Cos(phi));

                parallelsPts.Add(point);
                draw.Add(point);
            }

            Parallels[i-1] = parallelsPts;
        }


        //vertices.Add(N);
        //vertices.Add(S);

        /*for (int i = 0; i < nbMeridientsSph; i++)
        {
            triangles.Add(0);

            for (int j = 0; j < nbParallelesSph - 1 ; j += 2)
            {
                vertices.Add(Parallels[j][])


            }


            triangles.Add(1);
        }*/

        Debug.Log("Parallels.Length = " + Parallels.Length + "Parallels[0].Count= " + Parallels[0].Count);
        int t = 0;
        for (int i = 0; i < Parallels.Length -1; i++) {
            
            for (int j = 0; j < nbMeridientsSph -1 ; j++ , t += 4)
            {
                // A quad

                Debug.Log("Parallels["+i+"]["+j+"] ");

                vertices.Add(Parallels[i][j]);
                vertices.Add(Parallels[i + 1][j]);
                vertices.Add(Parallels[i][j + 1]);
                vertices.Add(Parallels[i + 1][j + 1]);


                triangles.Add(t);
                triangles.Add(t + 1);
                triangles.Add(t + 2);

                triangles.Add(t + 1);
                triangles.Add(t + 3);
                triangles.Add(t + 2);


            }

            //====== last ==============

            triangles.Add(t-2);
            triangles.Add(t-1);

            //Current problem ) is not valuable for every parallel
            triangles.Add(0);

            triangles.Add(t-1);
            triangles.Add(1);
            triangles.Add(0);

            //t = 0;

            Debug.Log("i = " + i);
        }

       myMF.mesh.vertices = vertices.ToArray();
       myMF.mesh.triangles = triangles.ToArray();
    }


    private IEnumerator vizualiser(Vector3 centre, List<Vector3>  vertices, List<int>  triangles, List<Vector3>[]  Parallels)
    {
        yield return new WaitForSeconds(2);

        for (int i = 1; i < nbParallelesSph; i++)  // from north pole to south pole
        {
            float phi = Mathf.PI * i / nbParallelesSph;  // goes from 0 to π

            List<Vector3> parallelsPts = new List<Vector3>();

            for (int j = 0; j < nbMeridientsSph; j++)  // around the sphere
            {
                float theta = 2 * Mathf.PI * j / nbMeridientsSph; // goes from 0 to 2π

                Vector3 point = new Vector3(centre.x + rayonSph * Mathf.Sin(phi) * Mathf.Cos(theta), centre.y + rayonSph * Mathf.Sin(phi) * Mathf.Sin(theta), centre.z + rayonSph * Mathf.Cos(phi));

                parallelsPts.Add(point);
                draw.Add(point);
                yield return new WaitForSeconds(1);
            }

            Parallels[i - 1] = parallelsPts;
        }

        for (int i = 0; i < nbParallelesSph - 2; i++)
        {

            for (int j = 0, t = 0; j < nbMeridientsSph - 1; j++, t += 4)
            {
                // A quad

                Debug.Log("Parallels[" + i + "][" + j + "] ");

                vertices.Add(Parallels[i][j]);
                vertices.Add(Parallels[i + 1][j]);
                vertices.Add(Parallels[i][j + 1]);

                vertices.Add(Parallels[i + 1][j]);
                vertices.Add(Parallels[i + 1][j + 1]);
                vertices.Add(Parallels[i][j + 1]);


                triangles.Add(t);
                triangles.Add(t + 1);
                triangles.Add(t + 2);

                triangles.Add(t + 2);
                triangles.Add(t + 1);
                triangles.Add(t + 3);

                yield return new WaitForSeconds(1);

            }
        }

        myMF.mesh.vertices = vertices.ToArray();
        myMF.mesh.triangles = triangles.ToArray();
    }



    private void OnDrawGizmos()
    {
        // Draw wire sphere outline.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(N, 0.3f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(S, 0.3f);

       

        for (int i = 0; i < draw.Count; i++) {
            if (i == 0) 
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(draw[i], 0.2f);
            }
            else if (i == 1)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(draw[i], 0.2f);
            }
            else if (i == draw.Count - 1) 
            {
                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(draw[i], 0.2f);
            }
            else
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(draw[i], 0.2f);
            }

        }
        

       
    }
}
