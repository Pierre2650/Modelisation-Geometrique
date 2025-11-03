using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
    private MeshFilter myMF;

    [Header("Plane")]
    public int nbLignes = 0, nbColones = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myMF = GetComponent<MeshFilter>();
        myMF.mesh.Clear();

    }

    public void generatePlane()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        int triIndex = 6;

        Vector3 startL = transform.position;

        for (int i = 0; i < nbLignes; i++)
        {
            Vector3 startC = startL;
            for (int j = 0; j < nbColones; j++)
            {
                generateQuad(startC, vertices, triangles, triIndex);
                startC = new Vector3(startC.x + 2, startC.y, startC.z);
                triIndex += 6;
            }

            startL = new Vector3(startL.x, startL.y, startL.z - 1);
        }


        myMF.mesh.vertices = vertices.ToArray();
        myMF.mesh.triangles = triangles.ToArray();
    }

    private void generateQuad(Vector3 start, List<Vector3> vertices, List<int> triangles, int trianglesIndex)
    {
        vertices.Add(start);
        vertices.Add(new Vector3(start.x, start.y, start.z + 1));
        vertices.Add(new Vector3(start.x + 2, start.y, start.z + 1));

        vertices.Add(start);
        vertices.Add(new Vector3(start.x + 2, start.y, start.z + 1));
        vertices.Add(new Vector3(start.x + 2, start.y, start.z));

        int i = triangles.Count;

        for (; i < trianglesIndex; i++)
        {
            Debug.Log("i triangles = " + i);
            triangles.Add(i);
        }

        Debug.Log("End");
    }

    public void resetMesh()
    {
        myMF.mesh.Clear();
    }
}
