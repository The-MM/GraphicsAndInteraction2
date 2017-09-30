using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    public GameObject brickDestroyedParticle;

    void OnCollisionEnter(Collision ball)
    {
        Instantiate(brickDestroyedParticle, transform.position, Quaternion.identity);
        GameManager.instance.DestroyedBrick();
        Destroy(gameObject);
    }
}
