using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleControl : MonoBehaviour {

    public float paddleSpeed = 0.5f;

    private Vector3 paddlePosition = new Vector3(0.0f, -9.5f, 0.0f);
    public static PaddleControl instance = null;

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
        paddlePosition = new Vector3(Mathf.Clamp(xPos, -8f, 8f), -9.5f, 0.0f);
        transform.position = paddlePosition;
	}

    // Changes the paddle's width
    public void ScalePaddle(float scale)
    {
        this.transform.localScale += new Vector3(0.0f, scale, 0.0f);
    }

}
