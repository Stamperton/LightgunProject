using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsShootable : MonoBehaviour, IShootable
{
    Rigidbody rBody;
    public Vector2 hitForce = new Vector2(45, 75);

    public void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public void OnGetHit(RaycastHit _hit, int weaponDamage)
    {
        rBody.isKinematic = false;
        rBody.AddForceAtPosition((transform.position - _hit.point) * Random.Range(hitForce.x, hitForce.y), _hit.point, ForceMode.Impulse);
    }

    public void OnGetHit(int weaponDamage)
    {
        rBody.isKinematic = false;
        rBody.AddForce(transform.position * Random.Range(hitForce.x, hitForce.y));
    }
}
