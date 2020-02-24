using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class SO_Weapon : ScriptableObject
{
    public PlayerWeaponType weaponType;
    public string weaponName;
    public int weapon_CurrentAmmo;
    public int weapon_ClipSize;
    public int damagePerBullet;
    public Vector2 weaponPhysicsForce;

    public AudioClip[] weaponAudio_Fire;
    public AudioClip[] weaponAudio_Reload;
    public AudioClip[] weaponAudio_DryFire;
    public AudioClip[] weaponAudio_Cock;

    public Sprite weaponIcon;
    public Sprite weaponIcon_Ammo;

    public bool isReloadable;
    public bool isAutomatic;
    public AudioClip weaponAudio_BulletTail;
    public float timeBetweenShots;
}

