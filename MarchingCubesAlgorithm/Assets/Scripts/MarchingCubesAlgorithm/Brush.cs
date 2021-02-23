using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    private Vector3 pointerPosition;

    public Camera activeCamera;


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
}
