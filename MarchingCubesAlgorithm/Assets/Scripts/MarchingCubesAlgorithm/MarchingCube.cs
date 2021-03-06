using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCube
{
    private int cubeType;
    private float isoLevel = 0.5f;

    private Vector3[] intersectionPoints;
    
    public Vector3[] IntersectionPoints
    {
        get { return intersectionPoints; }
    }

    private List<int[]> triangles;



    private Vector3 cubePosition;
    private Quaternion cubeRotation;
    private Vector3 cubeScale;

    public MarchingCube(float[] pointWeights, float level, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        cubePosition = pos;
        cubeRotation = rot;
        cubeScale = scale;

        isoLevel = level;

        cubeType = CalculateCubeType(pointWeights);
        intersectionPoints = CalculateIntersectionPoints(pointWeights);
        triangles = GetTriangles(cubeType);
    }


    public List<int> GetTris()
    {
        List<int> tris = new List<int>();

        foreach (var item in triangles)
        {
            tris.AddRange(item);
        }

        tris.Reverse();

        return tris;
    }


    public Mesh GetMesh()
    {
        Mesh outputMesh = new Mesh();

        outputMesh.vertices = intersectionPoints;

        for (int i = 0; i < intersectionPoints.Length; i++)
        {
            Debug.Log("index " + i + " = " + intersectionPoints[i]);
        }

        List<int> tris = new List<int>();

        foreach (var item in triangles)
        {
            tris.AddRange(item);
        }

        // tris.Reverse();

        outputMesh.triangles = tris.ToArray();

        return outputMesh;
    }


    List<int[]> GetTriangles(int cubeType)
    {
        List<int[]> triangles = new List<int[]>();

        for (int i = 0; i < 5; i++)
        {
            triangles.Add(new int[3]);
        }

        int[] subArray = Lookup.triTable[cubeType];

        for (int i = 0; i < subArray.Length - 1; i++)
        {
            triangles[Mathf.FloorToInt((float)i / 3f)][i % 3] = subArray[i];
        }

        triangles.RemoveAll(p => p[0] == -1 || p[1] == -1 || p[2] == -1);

        return triangles;
    }


    Vector3[] CalculateIntersectionPoints(float[] pointWeights)
    {
        Vector3[] output = new Vector3[12];

        bool[] intersectionEdges = Lookup.BoolArray(Lookup.edgeTable[cubeType]);

        for (int i = 0; i < 12; i++)
        {
            Vector3 intersectionPoint;

            if (intersectionEdges[i])
            {
                int point1Index = Lookup.edgePointIndex[i, 0];
                int point2Index = Lookup.edgePointIndex[i, 1];

                Vector3 point1Pos = Lookup.conerPositions[point1Index] / 2;
                Vector3 point2Pos = Lookup.conerPositions[point2Index] / 2;

                float point1Weight = pointWeights[point1Index];
                float point2Weight = pointWeights[point2Index];

                intersectionPoint = point1Pos + (isoLevel - point1Weight) * (point2Pos - point1Pos) / (point2Weight - point1Weight);

                intersectionPoint += cubePosition;
                intersectionPoint = cubeRotation * intersectionPoint;
            }
            else
            {
                intersectionPoint = Vector3.zero;
            }
            

            output[i] = intersectionPoint;
        }

        return output;
    }


    int CalculateCubeType(float[] pointWeights)
    {
        int type = 0;

        for (int edgeIndex = 0; edgeIndex < 8; edgeIndex++)
        {
            if (pointWeights[edgeIndex] > isoLevel)
            {
                type += (int)Mathf.Pow(2, edgeIndex);
            }
        }

        return type;
    }
}
