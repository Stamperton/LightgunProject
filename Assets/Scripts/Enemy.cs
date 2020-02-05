using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IShootable
{
    [HideInInspector] public Waypoint waypointSpawnedAt;

    //Are we Dead?
    bool isDead = false;

    public void OnDeath()
    {
        waypointSpawnedAt.enemiesAtThisWaypoint.Remove(this);
        this.gameObject.SetActive(false); 
    }

    public void OnGetHit(Vector3 hitPoint)
    {
        OnDeath();
    }
}
