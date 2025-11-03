using System.Collections.Generic;
using UnityEngine;

public class PacmanGenerator : MonoBehaviour
{
    private MeshFilter myMF;

    [Header("Sphere")]
    public int rayonSph;
    public int nbMeridientsSph;
    public int nbParallelesSph;
    private Vector3 N = Vector3.zero, S = Vector3.zero;
    private Vector3 centre;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myMF = GetComponent<MeshFilter>();
        myMF.mesh.Clear();
        centre = transform.position;

    }

    // Update is called once per frame
    public void generateSphereTronque()
    {
        if (nbMeridientsSph < 3 || nbParallelesSph < 2) { Debug.LogError("Nombre de meridiens  < 3 ou Nombre de paralleles < 2"); return; }

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector3>[] Parallels = new List<Vector3>[nbParallelesSph - 1];

        N = new Vector3(centre.x, centre.y, centre.z + rayonSph);
        S = new Vector3(centre.x, centre.y, centre.z - rayonSph);

        // =========  Points  =============
        for (int i = 1; i < nbParallelesSph; i++)  // meridians
        {
            float phi = Mathf.PI * i / nbParallelesSph;

            List<Vector3> parallelsPts = new List<Vector3>();

            for (int j = 0; j < nbMeridientsSph; j++)  // paralleles
            {
                float theta = 2 * Mathf.PI * j / nbMeridientsSph;

                Vector3 point = new Vector3(centre.x + rayonSph * Mathf.Sin(phi) * Mathf.Cos(theta), centre.y + rayonSph * Mathf.Sin(phi) * Mathf.Sin(theta), centre.z + rayonSph * Mathf.Cos(phi));

                parallelsPts.Add(point);
            }

            Parallels[i - 1] = parallelsPts;
        }

        vertices.Add(N);
        vertices.Add(S);

        int nbVertices = 2;
        for (int i = 0; i < Parallels.Length - 1; i++)
        {

            for (int j = 0; j < nbMeridientsSph - 1; j++, nbVertices += 4)
            {

                //========= Quads =============

                vertices.Add(Parallels[i][j]); // 0 // t
                vertices.Add(Parallels[i + 1][j]); // t+1
                vertices.Add(Parallels[i][j + 1]); // 1 // t+2
                vertices.Add(Parallels[i + 1][j + 1]); // t+3

                triangles.Add(nbVertices);
                triangles.Add(nbVertices + 1);
                triangles.Add(nbVertices + 2);

                triangles.Add(nbVertices + 1);
                triangles.Add(nbVertices + 3);
                triangles.Add(nbVertices + 2);

                //========= Poles =============
                //North Triangle
                if (i == 0)
                {

                    triangles.Add(0);
                    triangles.Add(nbVertices);
                    triangles.Add(nbVertices + 2);

                    triangles.Add(nbVertices + 2);
                    triangles.Add(1);
                    triangles.Add(0);
                }

                //South Triangle
                if (i == Parallels.Length - 2)
                {

                    triangles.Add(1);
                    triangles.Add(nbVertices + 3);
                    triangles.Add(nbVertices + 1);

                    triangles.Add(nbVertices + 1);
                    triangles.Add(0);
                    triangles.Add(1);
                }

            }

            //========= last Quad =============

            int lastTriangle = (nbMeridientsSph - 1) * 4;



            triangles.Add(nbVertices - 2);
            triangles.Add(nbVertices - 1);
            // triangles.Add(nbVertices - lastTriangle);
            triangles.Add(1);


            //triangles.Add(nbVertices - 1);
            triangles.Add(nbVertices + 1 - lastTriangle);
            triangles.Add(nbVertices - lastTriangle);
            triangles.Add(0);


        }


        myMF.mesh.vertices = vertices.ToArray();
        myMF.mesh.triangles = triangles.ToArray();

    }

    public void resetMesh()
    {
        myMF.mesh.Clear();
    }
}
