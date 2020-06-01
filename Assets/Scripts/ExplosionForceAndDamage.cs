using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionForceAndDamage : MonoBehaviour
{
    public float explosionRange;
    public int explosionDamage;
    public Vector2 explosionForce;

    public void Start()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, explosionRange);

        foreach (Collider nearbyObject in enemiesInRange)
        {
            Enemy _enemy = nearbyObject.GetComponentInParent<Enemy>();      

            _enemy?.OnGetHit(explosionDamage);

            if (explosionForce.x != 0 && explosionForce.y != 0)
            {
                Rigidbody _rbody = nearbyObject.GetComponent<Rigidbody>();
                if (_rbody != null)
                {
                    _rbody.AddForceAtPosition((transform.position) * Random.Range(explosionForce.x, explosionForce.y), transform.position, ForceMode.Impulse);

                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
