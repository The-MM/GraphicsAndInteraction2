using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleControl : MonoBehaviour {

    public float paddleSpeed = 10f;
    public AudioSource magnetAudio;
    public GameObject magnetParticlePrefab;
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
        // Stores values for unscaling later
        List<Vector3> childrenScale = new List<Vector3>();
        foreach (Transform child in this.transform)
        {
            childrenScale.Add(child.transform.localScale);
        }
        float parentX = this.transform.localScale.x;

        // Scales the paddle
        this.transform.localScale += new Vector3(scale, 0.0f, 0.0f);

        // Unscales the ball (using crazy maths I made)
        // For some reason the particle effect does not need unscaling
        int i = 0;
        foreach (Transform child in this.transform)
        {
            if(child.gameObject.name == "Ball")
            {
                child.transform.localScale = new Vector3((parentX * childrenScale[i].x) / (this.transform.localScale.x), childrenScale[i].y, childrenScale[i].z);
            }
            i++;
        }
    }

    // For a duration, sticks the ball to the paddle on contact
    public void MagnetModeOn(float duration)
    {
        if (!magnetMode)
        {
            magnetMode = true;
            magnetEndTime = Time.time + duration;

            // Turns on magnet sound and effects (check both children)
            magnetAudio.Play();
            if(this.transform.GetChild(0).name == "Magnet Particles")
            {
                this.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            }
            else
            {
                this.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            }
            
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
        if (this.transform.GetChild(0).name == "Magnet Particles")
        {
            this.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        }
        else
        {
            this.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        }

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
