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

    //UI Variables
    //ADD WEAPON ICON public Sprite currentWeaponIcon;
    public Text playerClipText; //BULLETS LEFT

    //ADD HEALTH

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponentInParent<PlayerShooting>();
    }

    public void FadeToBlack()
    {
        anim.SetTrigger("FadeOut");
    }

    public void UpdateUI()
    {
        //currentWeaponIcon.sprite = player.currentWeapon.weaponIcon;
        playerClipText.text = player.currentWeapon.weapon_CurrentAmmo.ToString();
    }
}
