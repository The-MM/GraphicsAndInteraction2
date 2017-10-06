using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private const int NORMAL_BALL_DMG = 1;
    private const int POWER_BALL_DMG = 3;

    public float initialVelocityEasy = 300f;
    public float initialVelocityMed = 600f;
    public float initialVelocityHard = 1000f;
    public int ballDamage = NORMAL_BALL_DMG;
    public Material ballMat;
    public Material rainbowMat;

    public bool ballInPlay = false;
    private bool powerBallMode = false;
    private Rigidbody rb;
    private float powerEndTime;

	public AudioSource powerBallAudio;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	void Update () {

        // Fire the ball
        if(Input.GetButtonDown("Fire1") && ballInPlay == false)
        {
            transform.parent = null;
            ballInPlay = true;
            rb.isKinematic = false;

            // Changes ball speed based on the difficulty level
            int difficulty = PlayerPrefs.GetInt("Difficulty Level");
            if (difficulty == 0)
            {
                rb.AddForce(new Vector3(initialVelocityEasy, initialVelocityEasy, 0.0f));
            }
            else if (difficulty == 1)
            {
                rb.AddForce(new Vector3(initialVelocityMed, initialVelocityMed, 0.0f));
            }
            else if (difficulty == 2)
            {
                rb.AddForce(new Vector3(initialVelocityHard, initialVelocityHard, 0.0f));
            }

        }

        if (powerBallMode && Time.time >= powerEndTime)
        {
            this.PowerModeOff();
        }
    }

    // Makes the ball go into power ball mode for a certain time duration
    public void PowerModeOn(float duration)
    {
        if(!powerBallMode)
        {
            powerBallMode = true;
            powerEndTime = Time.time + duration;
            ballDamage = POWER_BALL_DMG;

            // Swap music
            GameManager.instance.GetComponent<AudioSource>().Pause();
            powerBallAudio.Play();

            // Ignore all brick collisions
            GameObject bricks = GameManager.instance.GetBricks();
            for (int i = 0; i < bricks.transform.childCount; i++)
            {
                GameObject child = GameManager.instance.GetBricks().transform.GetChild(i).gameObject;
                child.GetComponent<Collider>().isTrigger = true;
            }

            // Turn on rainbow lighting
            this.GetComponent<Renderer>().material = rainbowMat;
        }

        // Refresh timer if already on
        else
        {
            powerEndTime = Time.time + duration;
        }
    }

    // Turns off power ball mode
    public void PowerModeOff()
    {
        this.GetComponent<Collider>().isTrigger = false;
        ballDamage = NORMAL_BALL_DMG;

        // Swap music back
		powerBallAudio.Stop();
        GameManager.instance.GetComponent<AudioSource>().UnPause();

        // Stops ignoring brick collisions
        GameObject bricks = GameManager.instance.GetBricks();
        for (int i = 0; i < bricks.transform.childCount; i++)
        {
            GameObject child = GameManager.instance.GetBricks().transform.GetChild(i).gameObject;
            child.GetComponent<Collider>().isTrigger = false;
        }

        // Turn off rainbow lighting
        this.GetComponent<Renderer>().material = ballMat;

        powerBallMode = false;
    }
}
