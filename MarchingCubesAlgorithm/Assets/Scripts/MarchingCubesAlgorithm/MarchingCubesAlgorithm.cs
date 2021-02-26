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


    FieldCreator fieldCreator;

    float[,,] field;


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

                    for (int cornerIndex = 0; cornerIndex < weight.Length; cornerIndex++)
                    {
                        Vector3Int indexPos = new Vector3Int(iX, iY, iZ) + Vector3Int.RoundToInt(Lookup.ConerPivotConerPositions()[cornerIndex]);
                        weight[cornerIndex] = field[indexPos.x, indexPos.y, indexPos.z];
                    }


                    marchingCubes.Add(new MarchingCube(weight, isoLevel, gameObject.transform.position + new Vector3(iX, iY, iZ) + Vector3.one * 0.5f, gameObject.transform.rotation, gameObject.transform.localScale));
                }
            }
        }
    }


    private void Update()
    {
        gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
    }


    private void Start()
    {
        fieldCreator = gameObject.GetComponent<FieldCreator>();

        field = fieldCreator.GeneratePerlinField();
    }


    private void OnValidate()
    {
        fieldCreator = gameObject.GetComponent<FieldCreator>();

        field = fieldCreator.GeneratePerlinField();
    }


    private void OnDrawGizmos()
    {
        Timer timer = new Timer("OnDrawGizmos");

        fieldCreator = gameObject.GetComponent<FieldCreator>();

        //field = fieldCreator.GeneratePerlinField();

        //fieldCreator.DrawFieldGizmo(field);


        Brush brush = gameObject.GetComponent<Brush>();

        fieldCreator.DrawFieldGizmo(brush.CalculateBrushweight(new Vector3Int(field.GetLength(0), field.GetLength(1), field.GetLength(2))));

        if (Input.GetMouseButton(0))
        {
            Debug.Log("Adding Terrain");
            field = brush.AddToFieldWeight(field, 1);
        }

        if (Input.GetMouseButton(1))
        {
            Debug.Log("Removeing Terrain");
            field = brush.AddToFieldWeight(field, -1);
        }


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

        timer.End();
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

        combinedMesh.SetVertices(verts);
        combinedMesh.triangles = tris.ToArray();

        List<Vector3> normalVectors = new List<Vector3>();

        for (int i = 0; i < verts.Count; i++)
        {
            normalVectors.Add(Vector3.one);
        }

        combinedMesh.normals = normalVectors.ToArray();

        return combinedMesh;
    }

    /*
    Mesh OptimizeMesh(Mesh unoptimizedMesh)
    {
        Vector3[] unoptimizedVertecies = unoptimizedMesh.vertices;
        int[] unoptimizedTriangles = unoptimizedMesh.triangles;
    }
    */


}
