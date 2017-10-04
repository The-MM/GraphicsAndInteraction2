using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    public GameManager.BrickTypes brickType;
    public GameObject createOnDestroy;

    void OnCollisionEnter(Collision ball)
    {
        this.GetComponent<HealthManager>().ApplyDamage(ball.gameObject.GetComponent<Ball>().ballDamage);
    }
}
