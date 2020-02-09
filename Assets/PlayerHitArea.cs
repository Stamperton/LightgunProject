﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.CompareTag("Enemy"))
        {
            other.GetComponentInParent<Enemy>().EnterAttackRange();
        }
    }
}
