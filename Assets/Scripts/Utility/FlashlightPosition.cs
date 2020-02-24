using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightPosition : MonoBehaviour
{
    public Camera cam;
    Light _light;

    void Start()
    {
        _light = GetComponent<Light>();
    }

    public void ToggleFlashlight()
    {
        bool toggle = _light.enabled;
        _light.enabled = !toggle;
    }
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
