using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCreator : MonoBehaviour
{
    private float[,,] fieldWeight;

    public Vector3Int fieldSize;

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

    public float[,,] GeneratePerlinField()
    {
        float[,,] field = new float[fieldSize.x, fieldSize.y, fieldSize.z];

        for (int iX = 0; iX < fieldSize.x; iX++)
        {
            for (int iY = 0; iY < fieldSize.y; iY++)
            {
                for (int iZ = 0; iZ < fieldSize.z; iZ++)
                {
                    //field[iX, iY, iZ] = Random.Range(0f, 1f);

                    if (iY < 2)
                    {
                        field[iX, iY, iZ] = 1;
                    }
                    else
                    {
                        field[iX, iY, iZ] = 0;
                    }
                    //field[iX, iY, iZ] = Mathf.PerlinNoise((float)iX / fieldSize.x, (float)iZ / fieldSize.z) * Mathf.PerlinNoise((float)iY / fieldSize.y, (float)iY / fieldSize.y);
                }
            }
        }

        return field;
    }

    public void DrawFieldGizmo(float[,,] field)
    {
        for (int iX = 0; iX < field.GetLength(0); iX++)
        {
            for (int iY = 0; iY < field.GetLength(1); iY++)
            {
                for (int iZ = 0; iZ < field.GetLength(2); iZ++)
                {
                    Gizmos.color = new Color(0, 0, 1, field[iX, iY, iZ]);
                    Gizmos.DrawCube(new Vector3(iX, iY, iZ), Vector3.one * 0.25f);
                }
            }
        }
    }
}
