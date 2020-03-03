using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager instance;

    public int playerHealth;

   void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("Multiple PlayerHealthManagers in Scene");
            Destroy(gameObject);
        }
    }
    
    PlayerUIManager playerUIManager;

    void Start()
    {
        playerUIManager = PlayerUIManager.instance;
    }

    public void TakeDamage(int amount)
    {
        playerHealth -= amount;

        //Update UI
        //Animate UI

        if (playerHealth <= 0)
        {
            Die();
        }
        //FEEDBACK IMPORTANT
    }

    void Die()
    {
        //DRAMATIC DEATH ANIMATION
    }

    public void HealDamage(int amount)
    {
        playerHealth += amount;
        //Animate UI
    }
}
