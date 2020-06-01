using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTarget : MonoBehaviour, IShootable
{
    public Animator anim;
    public bool isRoundTarget;
    public GameObject otherMenuTarget;

    public void OnGetHit(RaycastHit _hit, int weaponDamage)
    {
        ShootingRangeGameManager.instance.useRoundTargets = isRoundTarget;
        ShootingRangeGameManager.instance.StartNewGame();
        anim.SetBool("Ready", false);
        otherMenuTarget.GetComponent<Animator>().SetBool("Ready", false);
    }

    public void OnGetHit(int weaponDamage)
    {

    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        anim.SetBool("Ready", true);
    }
}
