using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    //Public Variables
    public Waypoint nextWaypoint;
    public List<Enemy> enemiesAtThisWaypoint = new List<Enemy>();

    [Header("Waypoint Variables")]
    [Tooltip("Movement Speed to next Waypoint in Units/Second")]
    public float moveSpeed; //Speed we move to next Camera


    [Tooltip("The time in seconds it takes to look completely at the new rotation")]
    public float lookSmoothing;


    [Tooltip("The time before the player moves to the next waypoint")]
    public float moveDelay;

    //Components
    Camera cam;


    void Start()
    {
        //Get Component References
        cam = GetComponent<Camera>();

        //Setup
        cam.enabled = false;

        //Give each Enemy a reference to this Waypoint. This will be used to modify the List.
        if (enemiesAtThisWaypoint.Count != 0)
        {
            foreach (Enemy enemy in enemiesAtThisWaypoint)
            {
                enemy.waypointSpawnedAt = this;
            }
        }

    }

    //Handle Enemy Delay Spawning
    //Handle Enemy Count

    private void Update()
    {


    }

    public Waypoint SetNextWaypointInGameManager()
    {
        return nextWaypoint;
    }

}
