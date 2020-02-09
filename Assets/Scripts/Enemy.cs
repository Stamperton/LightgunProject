using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IShootable
{
    //Components
    Animator anim;

    [HideInInspector] public Waypoint waypointSpawnedAt; //Waypoint Variable for this instance of Enemy

    [Header("Setup Variables")]
    //SpawnTimer
    [Tooltip("Time in Seconds before the enemy moves from Idle to Moving animation state")]
    public float spawnDelay; 

    //Collision Variables
    public ShootableArea[] shootableAreas;  //Public List of Colliders and HitLocation in Inspector
    Dictionary<Collider, HitLocation> _shootableAreas = new Dictionary<Collider, HitLocation>(); //Handling of the above

    HitLocation lastLocationHit; //Determines Animation played and damage taken

    EnemyState enemyState; //AI State Machine State

    [Header("Enemy Variables")]
    public int enemyHealth;
    //Are we Dead?
    bool isDead = false;

    public void Start()
    {
        //Setup Variables
        anim = GetComponent<Animator>();

        //Setup Collision Areas
        foreach (ShootableArea _area in shootableAreas)
        {
            _shootableAreas.Add(_area.collider, _area.hitLocation);
        }

        //Reset Variables

    }

    public void Spawn()
    {
        enemyState = EnemyState.Moving;
        AIHandler(enemyState);
    }

    public void EnterAttackRange()
    {
        enemyState = EnemyState.Attacking;
        AIHandler(enemyState);
    }

    public void Death()
    {
        isDead = true; //Dead!

        enemyState = EnemyState.Dead;
        AIHandler(enemyState);

        waypointSpawnedAt.enemiesAtThisWaypoint.Remove(this); //Remove from the Waypoint
    }

    public void OnGetHit(RaycastHit hitPoint, int weaponDamage)
    {
        _shootableAreas.TryGetValue(hitPoint.collider, out lastLocationHit);
        Debug.Log(lastLocationHit);

        //Handle Animation
        anim.SetTrigger("TakeDamage");
        // TODO : When Full Animations In - DamageAnimationHandler(lastLocationHit);

        //Handle Damage
        if (lastLocationHit == HitLocation.Head)
            enemyHealth -= (weaponDamage * 2);
        else
            enemyHealth -= weaponDamage;

        if (enemyHealth <= 0)
        {
            Death();
        }
    }

    void DamageAnimationHandler(HitLocation _hitLocation)
    {
        switch (_hitLocation)
        {
            case HitLocation.Null:
                break;
            case HitLocation.Head:
                break;
            case HitLocation.Torso:
                break;
            case HitLocation.LeftArm:
                break;
            case HitLocation.LeftLeg:
                break;
            case HitLocation.RightArm:
                break;
            case HitLocation.RightLeg:
                break;
            default:
                break;
        }
    }

    void AIHandler(EnemyState _state)
    {
        switch (_state)
        {
            case EnemyState.Idle:

                break;
            case EnemyState.Moving:
                anim.SetBool("Moving", true);
                break;
            case EnemyState.Attacking:
                anim.SetBool("Moving", false);
                anim.SetBool("Attacking", true);
                break;
            case EnemyState.Dead:
                anim.SetBool("Dead", true);
                break;
            default:
                break;
        }
    }

}
