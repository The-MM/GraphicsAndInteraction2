using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private const float POWER_UP_CHANCE = 0.4f;

    public float resetDelay = 1f;
    public int brickNum = 20;
    public int lifeNum = 3;
    public Text livesText;
    public Text gameOver;
    public Text youWin;
    public GameObject bricksPrefab;
    public GameObject paddlePrefab;
    public GameObject deathParticles;
    public GameObject powerUpPrefab;

    public static GameManager instance = null;

    public enum BrickTypes
    {
        RECTANGLE, TRIANGLE, CIRCLE
    }

    private GameObject paddle;
    private GameObject ball;
    private GameObject bricks;

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
        // On the first level, always reset lives (prevents closing game 
        // and restarting to increase lives)
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            PlayerPrefs.SetInt("Lives", lifeNum);
        }
        // Otherwise carry lives over from previous level
        else
        {
            lifeNum = PlayerPrefs.GetInt("Lives", lifeNum);
        }

        livesText.text = "Lives: " + lifeNum.ToString();
    }

    // Sets up paddle and bricks and music
    void InitialSetup()
    {
        SetupPaddle();
        bricks = Instantiate(bricksPrefab, transform.position, Quaternion.identity);
    }

    // Win or lose conditions checked
    void CheckGameOver()
    {
        if (brickNum <= 0)
        {
            // Save life number for next scene
            PlayerPrefs.SetInt("Lives", lifeNum);

            youWin.gameObject.SetActive(true);

            // Slow-mo
            Time.timeScale = .25f;

            Invoke("LoadNextLevel", resetDelay);
        }

		// You have died
        if (lifeNum <= 0)
        {

            gameOver.gameObject.SetActive(true);

            // Slow-mo
            Time.timeScale = .25f;

            // Score gets reset
            ScoreManager.instance.ResetScore();
			Invoke("LoadMainMenu", resetDelay);
        }

    }

	void LoadMainMenu(){
		Time.timeScale = 1f;
		SceneManager.LoadScene(0);
	}


    // Loads new level, or menu if at level 3 (last level)
    void LoadNextLevel()
    {
        Time.timeScale = 1f;

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        // Decrease lives
        IncrementLife(-1);

		CheckGameOver();
		Destroy(paddle);

		if (lifeNum > 0) {

			// Turn off power ball mode
			ball.GetComponent<Ball>().PowerModeOff();

			// Reset the paddle
			Instantiate(deathParticles, paddle.transform.position, Quaternion.identity);
			Invoke("SetupPaddle", resetDelay);
			
		}
    }

    // Changes the number of lives by a certain amount
    public void IncrementLife(int n)
    {
        lifeNum += n;
        livesText.text = "Lives: " + lifeNum.ToString();
    }

    // Instatiates new paddle
    void SetupPaddle()
    {
        paddle = Instantiate(paddlePrefab, transform.position, Quaternion.identity) as GameObject;
        ball = paddle.transform.GetChild(0).gameObject;
    }

    // A brick has been destroyed
    public void DestroyedBrick(Brick b)
    {
        // Increase score
        ScoreManager sm = ScoreManager.instance;
        sm.DestroyedBrick(b.brickType);

        // Has a chance to drop a power up at the brick's position
        if (Random.Range(0.0f, 1.0f) <= POWER_UP_CHANCE)
        {
            GameObject newPowerUp = Instantiate(powerUpPrefab, b.transform.position, Quaternion.identity);
            newPowerUp.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        }

        // Check if game should end
        brickNum--;
        CheckGameOver();
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Generates a random int (placed in GameManager so there's only one seed instance)
    public int RandomInt(int min, int max)
    {
        return Random.Range(min, max);
    }

    public GameObject GetPaddle()
    {
        return this.paddle;
    }

    public GameObject GetBall()
    {
        return this.ball;
    }

    public GameObject GetBricks()
    {
        return this.bricks;
    }
}
