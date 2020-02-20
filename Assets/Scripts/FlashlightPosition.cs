using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPosition : MonoBehaviour
{
    public Camera cam;

    private void Update()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(hit.point);         
        }
    }

}
