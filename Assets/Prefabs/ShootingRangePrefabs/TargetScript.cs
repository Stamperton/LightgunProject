using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour, IShootable
{
    TargetHitLocation locationHit;

    //Collision Variables
    public TargetShootableArea[] shootableAreas;  //Public List of Colliders and HitLocation in Inspector
    Dictionary<Collider, TargetHitLocation> _shootableAreas = new Dictionary<Collider, TargetHitLocation>(); //Handling of the above

    private void Start()
    {
        foreach (TargetShootableArea _area in shootableAreas)
        {
            _shootableAreas.Add(_area.collider, _area.hitLocation);
        }
    }

    public void OnGetHit(RaycastHit _hit, int weaponDamage)
    {
        _shootableAreas.TryGetValue(_hit.collider, out locationHit);
        Debug.Log(locationHit);
    }
}
