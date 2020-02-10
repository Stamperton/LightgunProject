using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    //Components
    AudioSource gunAudio;
    Camera cam;
    PlayerUIManager UIManager;

    public GameObject hitParticles;

    [Header("Weapon Variables")]
    RaycastHit _hit;
    public SO_Weapon defaultWeapon;
    public SO_Weapon currentWeapon;
    List<SO_Weapon> weaponList = new List<SO_Weapon>();

    //Logic Variables
    bool isReloading = false;
    float shotDelayTimer;

    //TODO : Weapon Switching

    private void Start()
    {
        //Initialise Components
        UIManager = PlayerUIManager.instance;
        gunAudio = GetComponent<AudioSource>();
        cam = GetComponentInChildren<Camera>();

        //Setup
        currentWeapon = defaultWeapon;
        currentWeapon.weapon_CurrentAmmo = currentWeapon.weapon_ClipSize;
        UIManager.UpdateUI();
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
        UIManager.UpdateUI();
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
        if (currentWeapon.weapon_CurrentAmmo > 0)
        {
            //Handle Audio and UI
            gunAudio.PlayOneShot(currentWeapon.weaponAudio_Fire[Random.Range(0, currentWeapon.weaponAudio_Fire.Length)]); //Play GunFire Audio
            if (currentWeapon.isAutomatic && currentWeapon.weapon_CurrentAmmo == 1)
                gunAudio.PlayOneShot(currentWeapon.weaponAudio_BulletTail); //Machinegun Audio Tail if Last Bullet

            currentWeapon.weapon_CurrentAmmo--;

            UIManager.UpdateUI();

            //Actually Shoot
            Debug.DrawRay(cam.ScreenPointToRay(Input.mousePosition).origin, cam.ScreenPointToRay(Input.mousePosition).direction, Color.red);
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out _hit, 100))
            {
                GameObject _particles = Instantiate(hitParticles, _hit.point, Quaternion.identity);
                Destroy(_particles, 1f);

                //Shootable Target?
                IShootable _targetShootable = _hit.collider.GetComponentInParent<IShootable>();

                if (_targetShootable != null)
                {
                    _targetShootable.OnGetHit(_hit, currentWeapon.damagePerBullet);
                }
            }

        }
        else
        {
            gunAudio.PlayOneShot(currentWeapon.weaponAudio_DryFire[Random.Range(0, currentWeapon.weaponAudio_DryFire.Length)]);
        }


    }
}
