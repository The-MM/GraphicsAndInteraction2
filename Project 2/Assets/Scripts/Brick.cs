using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    public GameManager.BrickTypes brickType;
    public GameObject createOnDestroy;

    // Collision with ball damages the brick
    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.GetComponent<Ball>().Equals(null))
        {
            this.GetComponent<HealthManager>().ApplyDamage(other.gameObject.GetComponent<Ball>().ballDamage);
        }
    }
}
