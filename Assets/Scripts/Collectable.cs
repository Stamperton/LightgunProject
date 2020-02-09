using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, IShootable
{
    public SO_Weapon weaponToCollect;
    public AudioClip collectionNoise;

    public void OnGetHit(RaycastHit _hit, int weaponDamage)
    {
        if (weaponToCollect != null)
        {
            GameManager.instance.player.GetComponent<PlayerShooting>().AddToWeaponList(weaponToCollect);
        }
        else
        {
            GameManager.instance.playerHealth++;
        }

        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().PlayOneShot(collectionNoise);

        this.gameObject.SetActive(false);
    }
}
