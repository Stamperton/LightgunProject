using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireJet : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponentInParent<Enemy>().EnterFire();
        }
    }
}
