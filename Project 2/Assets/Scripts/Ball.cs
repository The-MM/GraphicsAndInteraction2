using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public float initialVelocity = 600f;

    public int ballDamage = 1;

    private bool ballInPlay = false;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Fire1") && ballInPlay == false)
        {
            transform.parent = null;
            ballInPlay = true;
            rb.isKinematic = false;
            rb.AddForce(new Vector3(initialVelocity, initialVelocity, 0.0f));
        }
    }
}
