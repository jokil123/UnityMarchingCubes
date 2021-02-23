using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubesAlgorithm : MonoBehaviour
{
    public float isoLevel = 0.5f;

    public Material material;


    public float[][] weight = {
        new[] { 1f, 1f, 2f, 1f, 1f, 0f, 0f, 0f },
        new[] { 1f, 2f, 0f, 0f, 0f, 0f, 0f, 0f }};


    public Mesh mesh;

    public List<MarchingCube> marchingCubes = new List<MarchingCube>();



    private void CreateCubes(float[,,] field)
    {
        marchingCubes.Clear();

        for (int iX = 0; iX < field.GetLength(0) - 1; iX++)
        {
            for (int iY = 0; iY < field.GetLength(1) - 1; iY++)
            {
                for (int iZ = 0; iZ < field.GetLength(2) - 1; iZ++)
                {
                    float[] weight = new float[8];

                    weight[0] = field[iX, iY, iZ];
                    weight[1] = field[iX, iY, iZ + 1];
                    weight[2] = field[iX + 1, iY, iZ + 1];
                    weight[3] = field[iX + 1, iY, iZ];

                    weight[4] = field[iX, iY + 1, iZ];
                    weight[5] = field[iX, iY + 1, iZ + 1];
                    weight[6] = field[iX + 1, iY + 1, iZ + 1];
                    weight[7] = field[iX + 1, iY + 1, iZ];


                    marchingCubes.Add(new MarchingCube(weight, isoLevel, gameObject.transform.position + new Vector3(iX, iY, iZ) + Vector3.one * 0.5f, gameObject.transform.rotation, gameObject.transform.localScale));
                }
            }
        }
    }


    private void Update()
    {
        gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
    }




    private void OnDrawGizmos()
    {
        FieldCreator fieldCreator = gameObject.GetComponent<FieldCreator>();

        float[,,] field = fieldCreator.GeneratePerlinField();

        //fieldCreator.DrawFieldGizmo(field);

        CreateCubes(field);

        gameObject.GetComponent<MeshFilter>().mesh = GenerateCombinedMesh();

        foreach (MarchingCube cube in marchingCubes)
        {
            foreach (Vector3 item in cube.IntersectionPoints)
            {
                //Gizmos.color = new Color(0, 0, 1, 0.75f);
                //Gizmos.DrawCube(item, Vector3.one * 0.1f);
            }
        }
    }

    private void MakeCube()
    {
        marchingCubes = new List<MarchingCube>();

        marchingCubes.Add(new MarchingCube(weight[0], isoLevel, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.localScale));
        marchingCubes.Add(new MarchingCube(weight[1], isoLevel, gameObject.transform.position + Vector3.right, gameObject.transform.rotation, gameObject.transform.localScale));
    }

    private Mesh GenerateCombinedMesh()
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();


        for (int i = 0; i < marchingCubes.Count; i++)
        {
            verts.AddRange(marchingCubes[i].IntersectionPoints);

            List<int> cubetris = marchingCubes[i].GetTris();

            for (int i2 = 0; i2 < cubetris.Count; i2++)
            {
                cubetris[i2] += marchingCubes[i].IntersectionPoints.Length * i;
            }

            tris.AddRange(cubetris);
        }


        Mesh combinedMesh = new Mesh();

        combinedMesh.vertices = verts.ToArray();
        combinedMesh.triangles = tris.ToArray();

        return combinedMesh;
    }
}
