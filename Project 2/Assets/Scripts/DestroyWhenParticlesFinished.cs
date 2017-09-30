using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenParticlesFinished : MonoBehaviour
{
    public ParticleSystem targetParticleSystem;

    void Update()
    {
        if (!this.targetParticleSystem.IsAlive())
        {
            Destroy(targetParticleSystem.gameObject);
        }
    }
}
