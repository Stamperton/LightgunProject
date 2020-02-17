using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableLight : MonoBehaviour, IShootable
{
    public GameObject[] lights;

    public void OnGetHit(RaycastHit _hit, int weaponDamage)
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
    }
}
