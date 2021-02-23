using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public float brushRadius = 1;

    public float brushPressure = 1f;

    private Vector3 pointerPosition;

    private Camera activeCamera;


    private void Start()
    {
        activeCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        pointerPosition = GetMousePosition();
    }


    Vector3 GetMousePosition()
    {
        RaycastHit hit;
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);

        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            hitPosition = hit.point;
        }

        return hitPosition;
    }

    private void OnDrawGizmos()
    {
        //Debug.Log(pointerPosition);
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(pointerPosition, 0.25f);
    }


    public float[,,] CalculateBrushweight(Vector3Int size)
    {
        float[,,] brushWeight = new float[size.x, size.y, size.z];

        for (int iX = 0; iX < size.x; iX++)
        {
            for (int iY = 0; iY < size.y; iY++)
            {
                for (int iZ = 0; iZ < size.z; iZ++)
                {
                    brushWeight[iX, iY, iZ] = CalculateBrushweightAtPoint(new Vector3(iX, iY, iZ));
                }
            }
        }

        return brushWeight;
    }


    public float[,,] AddToFieldWeight(float[,,] inputField, float weightMultiplier)
    {
        float[,,] brushField = CalculateBrushweight(new Vector3Int(inputField.GetLength(0), inputField.GetLength(1), inputField.GetLength(2)));

        float[,,] outputField = new float[inputField.GetLength(0), inputField.GetLength(1), inputField.GetLength(2)];


        for (int iX = 0; iX < inputField.GetLength(0); iX++)
        {
            for (int iY = 0; iY < inputField.GetLength(0); iY++)
            {
                for (int iZ = 0; iZ < inputField.GetLength(0); iZ++)
                {
                    float weight = inputField[iX, iY, iZ] + brushField[iX, iY, iZ] * weightMultiplier;

                    outputField[iX, iY, iZ] = weight; //Mathf.Clamp(weight, 0, 1);
                }
            }
        }

        return outputField;
    }


    private float CalculateBrushweightAtPoint(Vector3 point)
    {
        float pointWeight;

        //pointWeight = brushRadius / (point - pointerPosition).magnitude;

        pointWeight = ((brushRadius - (point - pointerPosition).magnitude) / brushRadius) * brushPressure;
        pointWeight = Mathf.Max(pointWeight, 0);

        return pointWeight;
    }
}
