using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float resetDelay = 1f;
    public int brickNum = 20;
    public int lifeNum = 3;
    public Text livesText;
    public Text gameOver;
    public Text youWin;
    public GameObject bricksPrefab;
    public GameObject paddlePrefab;
    public GameObject deathParticles;
    public static GameManager instance = null;

    private GameObject paddle;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        InitialSetup();
    }

    private void Start()
    {
        livesText.text = "Lives: " + lifeNum.ToString();
    }

    // Sets up paddle and bricks
    void InitialSetup()
    {
        SetupPaddle();
        Instantiate(bricksPrefab, transform.position, Quaternion.identity);
    }

    // Win or lose conditions checked
    void CheckGameOver()
    {
        if (brickNum <= 0)
        {
            youWin.gameObject.SetActive(true);
            //Slow-mo
            Time.timeScale = .25f;
            Invoke("ResetLevel", resetDelay);
        }

        if (lifeNum <= 0)
        {
            gameOver.gameObject.SetActive(true);
            //Slow-mo
            Time.timeScale = .25f;
            Invoke("ResetLevel", resetDelay);
        }

    }

    // Resets the scene
    void ResetLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // The ball has gone out of play
    public void Died()
    {
        lifeNum--;
        livesText.text = "Lives: " + lifeNum.ToString();
        Instantiate(deathParticles, paddle.transform.position, Quaternion.identity);

        // Reset the paddle
        Destroy(paddle);
        Invoke("SetupPaddle", resetDelay);

        CheckGameOver();
    }

    // Instatiates new paddle
    void SetupPaddle()
    {
        paddle = Instantiate(paddlePrefab, transform.position, Quaternion.identity) as GameObject;
    }


    // A brick has been destroyed
    public void DestroyedBrick()
    {
        brickNum--;
        CheckGameOver();
    }
}