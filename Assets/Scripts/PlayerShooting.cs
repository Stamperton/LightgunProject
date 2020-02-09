using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    AudioSource gunAudio;

    RaycastHit _hit;

    Camera cam;

    [Header("Weapon Variables")]
    public SO_Weapon defaultWeapon;
    public SO_Weapon currentWeapon;
    List<SO_Weapon> weaponList = new List<SO_Weapon>();

    //Logic Variables
    bool isReloading = false;
    float shotDelayTimer;

    //TODO : List Current Weapons

    private void Start()
    {
        gunAudio = GetComponent<AudioSource>();
        cam = GetComponentInChildren<Camera>();
        currentWeapon = defaultWeapon;
        currentWeapon.weapon_CurrentAmmo = currentWeapon.weapon_ClipSize;
    }

    // Update is called once per frame
    private void Update()
    {
        //shotDelayTimer += Time.deltaTime;

        //Handle Weapon Shooting
        if (Input.GetMouseButtonDown(0) && !isReloading)
        {
            if (!currentWeapon.isAutomatic)
            {
                Shoot();
            }
            else
            {
                InvokeRepeating("Shoot", 0f, currentWeapon.timeBetweenShots);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("Shoot");
            if (currentWeapon.weapon_CurrentAmmo != 0)
                gunAudio.PlayOneShot(currentWeapon.weaponAudio_BulletTail); //Machinegun Bullet Tail (Echo Effect)
        }


        //Handle Reloading on any weapon that allows it
        if (Input.GetMouseButtonDown(1) && currentWeapon.isReloadable && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        AudioClip reloadAudio = currentWeapon.weaponAudio_Reload[Random.Range(0, currentWeapon.weaponAudio_Reload.Length)];
        gunAudio.PlayOneShot(reloadAudio);
        yield return new WaitForSeconds(reloadAudio.length);
        reloadAudio = currentWeapon.weaponAudio_Cock[Random.Range(0, currentWeapon.weaponAudio_Cock.Length)];
        gunAudio.PlayOneShot(reloadAudio);
        yield return new WaitForSeconds(reloadAudio.length);
        currentWeapon.weapon_CurrentAmmo = currentWeapon.weapon_ClipSize;
        isReloading = false;
    }

    public void AddToWeaponList(SO_Weapon weapon)
    {
        weaponList.Add(weapon);
        weapon.weapon_CurrentAmmo = weapon.weapon_ClipSize;
    }

    public void RemoveFromWeaponList(SO_Weapon weapon)
    {
        weaponList.Remove(weapon);
    }

    void Shoot()
    {
        Debug.DrawRay(cam.ScreenPointToRay(Input.mousePosition).origin, cam.ScreenPointToRay(Input.mousePosition).direction, Color.red);
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out _hit, 100))
        {

            if (currentWeapon.weapon_CurrentAmmo > 0)
            {
                gunAudio.PlayOneShot(currentWeapon.weaponAudio_Fire[Random.Range(0, currentWeapon.weaponAudio_Fire.Length)]); //Play GunFire Audio
                if (currentWeapon.isAutomatic && currentWeapon.weapon_CurrentAmmo == 1)
                    gunAudio.PlayOneShot(currentWeapon.weaponAudio_BulletTail); //Machinegun Audio Tail if Last Bullet


                currentWeapon.weapon_CurrentAmmo--;

                //Shootable Target?
                IShootable _targetShootable = _hit.collider.GetComponentInParent<IShootable>();

                if (_targetShootable != null)
                {
                    _targetShootable.OnGetHit(_hit, currentWeapon.damagePerBullet);
                }
            }
            else
            {
                gunAudio.PlayOneShot(currentWeapon.weaponAudio_DryFire[Random.Range(0, currentWeapon.weaponAudio_DryFire.Length)]);
            }
        }
    }
}
