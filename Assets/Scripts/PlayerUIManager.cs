using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    #region
    public static PlayerUIManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            Debug.Log("Muliple Player UI Managers in Scene");
        }
    }
    #endregion

    Animator anim;
    PlayerShooting player;

    //Screen Damage done through Animations

    //UI Variables
    public Image currentWeaponIcon;
    public Text playerClipText; //BULLETS LEFT
    public Text weaponNameText;

    //ADD HEALTH ICONS

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = PlayerShooting.instance;
    }

    public void FadeToBlack()
    {
        anim.SetTrigger("FadeOut");
    }

    public void UpdateUI()
    {
        currentWeaponIcon.sprite = player.currentWeapon.weaponIcon;
        playerClipText.text = player.currentWeapon.weapon_CurrentAmmo.ToString();
        weaponNameText.text = player.currentWeapon.weaponName.ToString();
    }
}
