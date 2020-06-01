using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableObjectWithAnimation : MonoBehaviour, IShootable
{
    public float health;

    public bool playOnce = true;
    bool hasPlayed = false;

    public void OnGetHit(RaycastHit _hit, int weaponDamage)
    {
        health -= weaponDamage;

        if (health <= 0)
        {
            Animate();
        }
    }

    public void OnGetHit(int weaponDamage)
    {
        health -= weaponDamage;

        if (health <= 0)
        {
            Animate();
        }
    }

    void Animate()
    {
        if (playOnce == true && hasPlayed)
            return;

        GetComponent<Animation>().Play();
        hasPlayed = true;
    }
}
