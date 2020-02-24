using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnHit: MonoBehaviour, IShootable
{
    public void OnGetHit(RaycastHit _hit, int weaponDamage)
    {
        GetComponent<Animator>().SetBool("Dead", true);
    }

}
