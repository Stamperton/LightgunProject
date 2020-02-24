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

    public GameObject flashlightGO;
    public GameObject hitParticles;
    public GameObject flamethrowerParticles;

    [Header("Weapon Variables")]
    RaycastHit _hit;
    public SO_Weapon defaultWeapon;
    public SO_Weapon currentWeapon;
    List<SO_Weapon> weaponList = new List<SO_Weapon>();

    //Weapon List Variables
    public int currentWeaponListPosition = 0;

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
        weaponList.Add(defaultWeapon);
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

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            CycleWeapons(-1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            CycleWeapons(1);
        }


        //Handle Reloading on any weapon that allows it
        if (Input.GetMouseButtonDown(1) && currentWeapon.isReloadable && !isReloading)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlightGO.SetActive(!flashlightGO.activeSelf);
        }

    }

    IEnumerator Reload()
    {
        //Prevent Multiple Reloads at once
        isReloading = true;

        //Plays two seperate Audio Clips that are randomised - One for the magazine, one for the gun slide.
        AudioClip reloadAudio = currentWeapon.weaponAudio_Reload[Random.Range(0, currentWeapon.weaponAudio_Reload.Length)];
        gunAudio.PlayOneShot(reloadAudio);
        yield return new WaitForSeconds(reloadAudio.length);
        reloadAudio = currentWeapon.weaponAudio_Cock[Random.Range(0, currentWeapon.weaponAudio_Cock.Length)];
        gunAudio.PlayOneShot(reloadAudio);
        yield return new WaitForSeconds(reloadAudio.length);

        //Reload appropriate amount of ammunition and update UI. This is the last thing done so the player cannot shoot mid-reload
        currentWeapon.weapon_CurrentAmmo = currentWeapon.weapon_ClipSize;
        UIManager.UpdateUI();

        //Re-enables reloading.
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

            //debug for testing
            Debug.DrawRay(cam.ScreenPointToRay(Input.mousePosition).origin, cam.ScreenPointToRay(Input.mousePosition).direction, Color.red);

            //Actually Shoot
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out _hit, 100))
            {
                WeaponParticles(currentWeapon.weaponType);

                //Shootable Target?
                IShootable _targetShootable = _hit.collider.GetComponentInParent<IShootable>();

                _targetShootable?.OnGetHit(_hit, currentWeapon.damagePerBullet);


            }
        }
        else
        {
            gunAudio.PlayOneShot(currentWeapon.weaponAudio_DryFire[Random.Range(0, currentWeapon.weaponAudio_DryFire.Length)]);
        }
    }

    void WeaponParticles(PlayerWeaponType weaponType)
    {
        GameObject _particles;
        if (weaponType != PlayerWeaponType.Flamethower)
        {
            _particles = Instantiate(hitParticles, _hit.point, Quaternion.identity);
        }
        else
        {
            _particles = Instantiate(flamethrowerParticles, cam.transform.position, Quaternion.LookRotation(_hit.transform.position));
            //_particles.transform.LookAt(_hit.transform);
        }
        Destroy(_particles, 1f);
    }

    void CycleWeapons(int cycle)
    {
        currentWeaponListPosition += cycle;
        if (currentWeaponListPosition < 0)
        {
            currentWeaponListPosition = weaponList.Count - 1;
        }
        if (currentWeaponListPosition > weaponList.Count - 1)
        {
            currentWeaponListPosition = 0;
        }

        currentWeapon = weaponList[currentWeaponListPosition];

        UIManager.UpdateUI();
    }
}
