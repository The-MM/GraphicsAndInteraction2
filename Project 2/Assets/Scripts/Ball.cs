using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private const int NORMAL_BALL_DMG = 1;
    private const int POWER_BALL_DMG = 3;

    public float initialVelocity = 600f;
    public int ballDamage = NORMAL_BALL_DMG;
    public Material ballMat;
    public Material rainbowMat;

    private bool ballInPlay = false;
    private bool powerBallMode = false;
    private Rigidbody rb;
    private float endTime;

	public AudioSource audio;

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
            rb.AddForce(new Vector3(initialVelocity, initialVelocity, 0.0f));
        }

        if (powerBallMode && Time.time >= endTime)
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
            endTime = Time.time + duration;
            ballDamage = POWER_BALL_DMG;

            // Start music
            audio.Play();

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
            endTime = Time.time + duration;
        }
    }

    // Turns off power ball mode
    public void PowerModeOff()
    {
        this.GetComponent<Collider>().isTrigger = false;
        ballDamage = NORMAL_BALL_DMG;

        // Stop music
		audio.Stop();

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
