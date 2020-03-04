using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosion;
    public GameObject grenade;

    void OnTriggerEnter(Collider other)
    {
        GameObject _explosion = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(_explosion.gameObject, 5f);
        grenade.SetActive(false);
    }
}
