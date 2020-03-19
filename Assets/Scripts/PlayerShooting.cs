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

    [Header("Weapon Variables")]
    RaycastHit _hit;
    public GameObject grenadePrefab;
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

            if (currentWeapon.weaponType == PlayerWeaponType.GrenadeLauncher)
            {
                GameObject _grenade = Instantiate(grenadePrefab, cam.transform.position, cam.transform.rotation);
                _grenade.GetComponent<Rigidbody>().AddForce(cam.ScreenPointToRay(Input.mousePosition).direction * 20f, ForceMode.Impulse);
            }

            else //Raycast Shooting
            {
                //Actually Shoot
                if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out _hit, 100))
                {
                    Debug.DrawLine(cam.transform.position, _hit.point, Color.yellow, 1f);

                    if (currentWeapon.weaponType == PlayerWeaponType.Shotgun)
                    {
                        int shotCount = Random.Range(8, 12);
                        for (int i = 0; i < shotCount; i++)
                        {
                            ShotgunRay();
                        }
                        return;
                    }
                    WeaponImpactParticles(currentWeapon.weaponType, _hit.point);
                    WeaponFireParticles(currentWeapon.weaponFireParticles, _hit.point);

                    //Shootable Target?
                    IShootable _targetShootable = _hit.collider.GetComponentInParent<IShootable>();

                    _targetShootable?.OnGetHit(_hit, currentWeapon.damagePerBullet);


                }
            }
        }
        else
        {
            gunAudio.PlayOneShot(currentWeapon.weaponAudio_DryFire[Random.Range(0, currentWeapon.weaponAudio_DryFire.Length)]);
        }
    }

    void ShotgunRay()
    {
        Vector3 direction = _hit.point - transform.position; // your initial aim.
        Vector3 spread = Vector3.zero;
        spread += cam.transform.up * Random.Range(-1f, 1f); // add random up or down (because random can get negative too)
        spread += cam.transform.right * Random.Range(-1f, 1f); // add random left or right

        // Using random up and right values will lead to a square spray pattern. If we normalize this vector, we'll get the spread direction, but as a circle.
        // Since the radius is always 1 then (after normalization), we need another random call. 
        direction += spread.normalized * Random.Range(0, 2);

        RaycastHit shotgunHit;

        if (Physics.Raycast(cam.transform.position, direction, out shotgunHit, 100))
        {
            {
                Debug.DrawLine(cam.transform.position, shotgunHit.point, Color.green, 1f);


                IShootable _targetShootable = shotgunHit.collider.GetComponentInParent<IShootable>();
                _targetShootable?.OnGetHit(shotgunHit, currentWeapon.damagePerBullet);

                WeaponFireParticles(currentWeapon.weaponFireParticles, shotgunHit.point);
                WeaponImpactParticles(currentWeapon.weaponType, shotgunHit.point);
            }
        }

    }
    void WeaponImpactParticles(PlayerWeaponType weaponType, Vector3 particleLocation)
    {
        //TODO : PARTICLE POOL

        GameObject _particles;

        _particles = Instantiate(hitParticles, particleLocation, Quaternion.identity);

        Destroy(_particles, 1f);
    }

    void WeaponFireParticles(GameObject _bulletParticle, Vector3 destination)
    {
        if (_bulletParticle == null)
            return;

        //TODO : WEAPON BULLET POOLING
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
