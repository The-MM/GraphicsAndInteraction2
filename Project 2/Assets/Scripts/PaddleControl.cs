using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleControl : MonoBehaviour {

    public float paddleSpeed = 1000f;

    private Vector3 paddlePosition = new Vector3(0.0f, -9.5f, 0.0f);
	
	// Update is called once per frame
	void Update () {
        float xPos = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeed * Time.deltaTime);
        paddlePosition = new Vector3(Mathf.Clamp(xPos, -8f, 8f), -9.5f, 0.0f);
        transform.position = paddlePosition;
	}
}
