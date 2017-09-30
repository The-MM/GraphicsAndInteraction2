using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour {

    private void OnTriggerEnter(Collider ball)
    {
        GameManager.instance.Died();
    }
}
