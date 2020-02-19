using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IShootable
{
    public float explosionRange;
    public int explosionDamage;

    public GameObject fireparticles;

    bool hit = false;
    int hitcount = 0;

    public void OnGetHit(RaycastHit _hit, int weaponDamage)
    {

        if (hitcount == 0)
        {
            hitcount++;
            fireparticles.SetActive(true);
            Debug.Log("FThrow");
            return;
        }

        hitcount++;

        GetComponent<Animator>().enabled = true;
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
        Gizmos.DrawSphere(fireparticles.transform.position, .2f);
    }
}
