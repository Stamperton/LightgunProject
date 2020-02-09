using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Multiple Game Managers in Scene");
            Destroy(gameObject);
        }
    }

    #endregion

    //Player Variables
    public GameObject player;
    public AnimationCurve playerAnimationCurve;
    Animator playerAnimator;

    public int playerHealth;

    public Waypoint startingWaypoint;
    public Waypoint currentWaypoint;

    //Movement Variables
    Transform startPosition;
    float moveDelayTimer;

    public bool moveToWaypoint;
    bool lookToWaypointDirection;

    //Animation Curve Variables
    bool initialiseMove = true;
    float moveX;
    float moveY;
    float rotX;
    float rotY;



    void Start()
    {
        playerAnimator = player.GetComponent<Animator>();

        currentWaypoint = startingWaypoint;

        player.transform.position = startingWaypoint.transform.position;
        player.transform.rotation = startingWaypoint.transform.rotation;

        moveToWaypoint = true;

    }

    //COOME BACK TO THIS STUFF

    public void Update()
    {
        if (currentWaypoint.enemiesAtThisWaypoint.Count == 0 && currentWaypoint.nextWaypoint != null) //Move only if there are no enemies, or you actually have somewhere to move. Idiot.
        {
            moveDelayTimer += Time.deltaTime;
            moveToWaypoint = true;
        }
        if (moveDelayTimer >= currentWaypoint.moveDelay)
        {
            MoveBetweenWaypoints();
        }
    }


    public void MoveBetweenWaypoints()
    {
        if (moveToWaypoint)
        {
            #region OldCode
            //playerAnimator.SetBool("PlayerWalking", true); //Animate Player Movement

            //player.transform.position = Vector3.MoveTowards(player.transform.position, currentWaypoint.nextWaypoint.transform.position, currentWaypoint.moveSpeed * Time.deltaTime); //Move Player

            //if (player.transform.position == currentWaypoint.nextWaypoint.transform.position) //Finished Moving?
            //{
            //    lookToWaypointDirection = true; //Stop Moving
            //    moveToWaypoint = false;
            //    playerAnimator.SetBool("PlayerWalking", false); //Stop Walking Animation
            //   }
            #endregion

            if (initialiseMove)
            {
                Debug.Log("moveStarted!");
                initialiseMove = false;
                moveX = 0;
                moveY = 0;
                rotX = 0;
                rotY = 0;
                startPosition = player.transform;
            }

            moveY = playerAnimationCurve.Evaluate(moveX);
            player.transform.position = Vector3.Lerp(startPosition.position, currentWaypoint.nextWaypoint.transform.position, moveY);
            moveX += Time.deltaTime * (1 / (Vector3.Distance(startPosition.transform.position, currentWaypoint.nextWaypoint.transform.position) / currentWaypoint.moveSpeed)); //Black fucking magic. Somehow converts to Units/Second.

            if (moveX >= 1)
            {
                moveX = 1;
                moveToWaypoint = false;
                lookToWaypointDirection = true;
            }

        }
        if (lookToWaypointDirection)
        {
            rotY = playerAnimationCurve.Evaluate(rotX);
            player.transform.rotation = Quaternion.Slerp(startPosition.rotation, currentWaypoint.nextWaypoint.transform.rotation, rotY);
            rotX += Time.deltaTime * (1 / (Quaternion.Angle(startPosition.rotation, currentWaypoint.nextWaypoint.transform.rotation) / currentWaypoint.lookSmoothing));

            if (player.transform.rotation == currentWaypoint.nextWaypoint.transform.rotation)
            {
                initialiseMove = true;
                lookToWaypointDirection = false;
                UpdateWaypoint();
            }
        }
    }


    void UpdateWaypoint()
    {
        currentWaypoint = currentWaypoint.SetNextWaypointInGameManager();

        InitialiseEnemiesAtWaypoint(currentWaypoint);

        moveDelayTimer = 0f;

    }

    public void InitialiseEnemiesAtWaypoint(Waypoint _waypoint)
    {
        foreach (Enemy enemy in currentWaypoint.enemiesAtThisWaypoint)
        {
            StartCoroutine(EnemySpawnDelay(enemy.spawnDelay, enemy));
        }
    }

    IEnumerator EnemySpawnDelay(float timeDelay, Enemy enemy)
    {
        yield return new WaitForSeconds(timeDelay);

        enemy.Spawn();
    }

}
