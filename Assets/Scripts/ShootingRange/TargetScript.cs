using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour, IShootable
{
    //Components
    public Animator animator;

    //Timer Handling
    bool timerRunning;
    float timer;

    //Used to determine Score
    TargetHitLocation locationHit;
    bool isActive;

    //Collision Variables & Setup
    public TargetShootableArea[] shootableAreas;  //Public List of Colliders and HitLocation in Inspector
    Dictionary<Collider, TargetHitLocation> _shootableAreas = new Dictionary<Collider, TargetHitLocation>(); //Handling of the above

    private void Start()
    {
        foreach (TargetShootableArea _area in shootableAreas)
        {
            _shootableAreas.Add(_area.collider, _area.hitLocation);
        }
    }

    private void Update()
    {
        //Handle Target "Up" time
        if (timerRunning)
        {
            timer += Time.deltaTime;

            if (timer >= ShootingRangeGameManager.instance.timeTargetsActive)
            {
                SetTargetInactive();
            }
        }
    }

    void ResetTimer()
    {
        timer = 0f;
        timerRunning = false;
    }

    public void OnGetHit(RaycastHit _hit, int weaponDamage)
    {
        if (!isActive)
            return;
        isActive = false;
        _shootableAreas.TryGetValue(_hit.collider, out locationHit);
        SetTargetInactive();
        Score(locationHit);
        ResetTimer();
    }

    public void OnGetHit(int weaponDamage)
    {
        return;
    }

    public void SetTargetActive()
    {
        Debug.Log(name + " set active");
        isActive = true;
        animator.SetBool("Ready", true);
        ResetTimer();
        timerRunning = true;

    }

    public void SetTargetInactive()
    {
        animator.SetBool("Ready", false);
        isActive = false;
        ShootingRangeGameManager.instance.GetNewTarget();
        ResetTimer();
    }

    public void Score(TargetHitLocation _hit)
    {
        int _score = 0;

        switch (_hit)
        {
            case TargetHitLocation.Edge:
                _score = 2;
                break;
            case TargetHitLocation.Outer:
                _score = 4;
                break;
            case TargetHitLocation.Middle:
                _score = 6;
                break;
            case TargetHitLocation.Inner:
                _score = 8;
                break;
            case TargetHitLocation.Center:
                _score = 10;
                break;
            default:
                break;
        }

        ShootingRangeGameManager.instance.UpdateScore(_score);
    }
}
