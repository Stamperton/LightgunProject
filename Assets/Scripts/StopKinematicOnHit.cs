using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopKinematicOnHit : MonoBehaviour, IShootable
{
    public void OnGetHit(RaycastHit _hit, int weaponDamage)
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
