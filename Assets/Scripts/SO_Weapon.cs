using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class SO_Weapon : ScriptableObject
{
    public string weaponName;
    public int weapon_ClipSize;
    public int damagePerBullet;

    public AudioClip[] weaponAudio_Fire;
    public AudioClip[] weaponAudio_Reload;
    public AudioClip[] weaponAudio_DryFire;

    public Sprite weaponIcon;
    public Sprite weaponIcon_Ammo;
    
}

