using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour, IShootable
{
    Rigidbody rBody;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public void OnGetHit(RaycastHit _hit, int weaponDamage)
    {
        transform.SetParent(null);
        rBody.isKinematic = false;
        rBody.AddForce(_hit.point * 1, ForceMode.Impulse);
    }
}
