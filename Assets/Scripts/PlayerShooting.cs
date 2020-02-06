using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    RaycastHit _hit;

    Camera cam;

    [Header("Weapon Variables")]
    public SO_Weapon defaultWeapon;
    public SO_Weapon currentWeapon;

    //TODO : List Current Weapons

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        currentWeapon = defaultWeapon;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.DrawRay(cam.ScreenPointToRay(Input.mousePosition).origin, cam.ScreenPointToRay(Input.mousePosition).direction, Color.red);
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out _hit, 100))
            {
                  //Shootable Target?
                IShootable _targetShootable = _hit.collider.GetComponentInParent<IShootable>();

                if (_targetShootable != null)
                {
                    _targetShootable.OnGetHit(_hit, currentWeapon.damagePerBullet);
                }
            }
        }
    }
}
