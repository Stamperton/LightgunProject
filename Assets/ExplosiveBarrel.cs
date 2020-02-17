using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IShootable
{
    public float explosionRange;
    public int explosionDamage;

    public void OnGetHit(RaycastHit _hit, int weaponDamage)
    {
        GetComponent<Animator>().SetBool("Dead", true);


        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, explosionRange);

        foreach (Collider nearbyObject in enemiesInRange)
        {
            Enemy _enemy = nearbyObject.GetComponentInParent<Enemy>();
            if (_enemy != null)
            {
                _enemy.OnGetHit(_hit, explosionDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
