using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    public GameManager.BrickTypes brickType;
    public GameObject createOnDestroy;

    // Collision with ball damages the brick
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Ball>() != null)
        {
            this.GetComponent<HealthManager>().ApplyDamage(other.gameObject.GetComponent<Ball>().ballDamage);
        }
    }
    
    // Used for power ball mode (when bricks are triggers)
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ball>() != null)
        {
            this.GetComponent<HealthManager>().ApplyDamage(other.gameObject.GetComponent<Ball>().ballDamage);
        }
    }
}
