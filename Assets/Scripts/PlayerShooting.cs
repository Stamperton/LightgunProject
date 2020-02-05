using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    RaycastHit _hit;

    Camera cam;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out _hit, 100))
            {
                  //Shootable Target?
                IShootable _targetShootable = _hit.collider.GetComponent<IShootable>();

                if (_targetShootable != null)
                {
                    _targetShootable.OnGetHit(_hit.point);
                    Debug.Log("Shot the " + _hit.collider);
                }
            }
        }
    }
}
