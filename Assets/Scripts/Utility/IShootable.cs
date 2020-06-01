using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    void OnGetHit(RaycastHit _hit, int weaponDamage);
    void OnGetHit(int weaponDamage);
}
