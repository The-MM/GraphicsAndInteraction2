using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleControl : MonoBehaviour {

    public float paddleSpeed = 10f;
    public AudioSource magnetAudio;
    public static PaddleControl instance = null;

    private Vector3 paddlePosition = new Vector3(0.0f, -9.5f, 0.0f);
    private bool magnetMode = false;
    private float magnetEndTime;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {

        // Moves based on user input
        float xPos = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeed);
        paddlePosition = new Vector3(Mathf.Clamp(xPos, -13f, 13f), -9.5f, 0.0f);
        transform.position = paddlePosition;

        if (magnetMode && Time.time >= magnetEndTime)
        {
            this.MagnetModeOff();
        }
    }

    // Changes the paddle's width
    public void ScalePaddle(float scale)
    {
        this.transform.localScale += new Vector3(scale, 0.0f, 0.0f);
    }

    // For a duration, sticks the ball to the paddle on contact
    public void MagnetModeOn(float duration)
    {
        if (!magnetMode)
        {
            magnetMode = true;
            magnetEndTime = Time.time + duration;

            // Turns on magnet sound and effects
            magnetAudio.Play();
            //TODO
        }

        // Refresh timer if already on
        else
        {
            magnetEndTime = Time.time + duration;
        }

    }

    // Attaches the ball to the paddle
    private void AttachBall()
    {
        GameManager.instance.GetBall().transform.parent = this.gameObject.transform;
        GameManager.instance.GetBall().GetComponent<Ball>().ballInPlay = false;
        GameManager.instance.GetBall().GetComponent<Rigidbody>().isKinematic = true;
    }

    // Turns off magnet mode
    private void MagnetModeOff()
    {
        // Turn off magnet sound and effects
        magnetAudio.Stop();
        //TODO

        magnetMode = false;
    }

    // Stick to the ball if magnet mode is on
    void OnCollisionEnter(Collision collision)
    {
        if (magnetMode && collision.collider.gameObject.GetComponent<Ball>() != null)
        {
            AttachBall();
        }
    }

}
