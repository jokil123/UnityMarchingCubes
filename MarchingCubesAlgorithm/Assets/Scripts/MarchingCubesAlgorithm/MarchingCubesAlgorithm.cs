using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubesAlgorithm : MonoBehaviour
{
    private float[,,] fieldWeight;

    public float isoLevel = 0.5f;

    public Vector3Int fieldSize;

    public Material material;

    public MarchingCube a;

    public float[] weight = { 1, 0, 1, 0, 1, 0, 1, 0 };

    public Mesh mesh;

    void SetupField()
    {
        fieldWeight = new float[fieldSize.x, fieldSize.y, fieldSize.z];

        for (int iX = 0; iX < fieldSize.x; iX++)
        {
            for (int iY = 0; iY < fieldSize.y; iY++)
            {
                for (int iZ = 0; iZ < fieldSize.z; iZ++)
                {
                    fieldWeight[iX, iY, iZ] = 0;
                }
            }
        }
    }


    void GeneratePerlinField()
    {
        for (int iX = 0; iX < fieldSize.x; iX++)
        {
            for (int iY = 0; iY < fieldSize.y; iY++)
            {
                for (int iZ = 0; iZ < fieldSize.z; iZ++)
                {
                    fieldWeight[iX, iY, iZ] = Random.Range(0f, 1f);
                    // Debug.Log(fieldWeight[iX, iY, iZ]);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        MakeCube();
        gameObject.GetComponent<MeshFilter>().mesh = a.GetMesh();

        foreach (Vector3 item in a.intersectionPoints)
        {
            Gizmos.color = new Color(1, 0, 0, 0.75f);
            Gizmos.DrawCube(item, Vector3.one * 0.1f);
            // Debug.DrawLine(Vector3.zero, item, Color.blue, Time.deltaTime, true);
        }
    }

    private void MakeCube()
    {
        a = new MarchingCube(weight, isoLevel, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.localScale);
    }
}
