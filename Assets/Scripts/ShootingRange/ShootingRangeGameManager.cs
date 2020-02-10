using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingRangeGameManager : MonoBehaviour
{
    #region Singleton
    public static ShootingRangeGameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    #endregion

    public GameObject roundMenu;
    public GameObject outlineMenu;

    public GameObject[] roundTargets;
    public GameObject[] outlineTargets;


    //UI and Score variables
    [HideInInspector] public bool gameRunning;
    public int currentScore;
    public Text scoreText;
    public Text highScoreText;
    public Text timerText;

    float gameTimer;

    [Header("Modifiers")]
    public float gameLength = 60f;
    public bool useRoundTargets;
    public float timeBetweenTargets = 0.25f;
    public float timeTargetsActive = 2f;

    private void Update()
    {
        if (gameRunning)
            gameTimer = Mathf.Clamp(gameTimer -= Time.deltaTime, 0, 60);
        if (gameTimer <= 0 &&gameRunning)
            EndGame();

        timerText.text = ("Time : " + gameTimer.ToString("##.##"));

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    //Initialisation
    public void StartNewGame()
    {
        gameRunning = true;
        gameTimer = gameLength;
        SetHighScore(currentScore);
        currentScore = 0;
        scoreText.text = ("SCORE : " + currentScore.ToString());
        GetNewTarget();
    }

    public void EndGame()
    {
        Debug.Log("Endgame");
        gameRunning = false;
        SetHighScore(currentScore);
        StartCoroutine(ResetMenuTargets());
    }

    public void UpdateScore(int _score)
    {
        currentScore += _score;
        scoreText.text = ("SCORE : " + currentScore.ToString());
    }

    public void GetNewTarget()
    {
        if (!gameRunning)
            return;
        StartCoroutine(NewTargetWithDelay());
    }

    IEnumerator NewTargetWithDelay()
    {
        yield return new WaitForSeconds(timeBetweenTargets);
        if (useRoundTargets)
        {
            int newRoundTarget = Random.Range(0, roundTargets.Length);
            roundTargets[newRoundTarget].GetComponent<TargetScript>().SetTargetActive();
        }
        if (!useRoundTargets)
        {
            int newOutlineTarget = Random.Range(0, outlineTargets.Length);
            outlineTargets[newOutlineTarget].GetComponent<TargetScript>().SetTargetActive();
        }

    }

    IEnumerator ResetMenuTargets()
    {
        yield return new WaitForSeconds(2f);
        outlineMenu.GetComponent<MenuTarget>().Reset();
        roundMenu.GetComponent<MenuTarget>().Reset();
    }

    void SetHighScore(int _score)
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (_score > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", _score);
        }
        highScoreText.text = ("High Score : " + PlayerPrefs.GetInt("HighScore").ToString());
    }
}
