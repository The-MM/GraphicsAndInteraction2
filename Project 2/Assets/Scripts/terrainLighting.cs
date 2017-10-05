using UnityEngine;
using System.Collections;

public class terrainLighting : MonoBehaviour
{
    public PointLight pointLight;

    // Called each frame
    void Update()
    {
        // Get renderer component (in order to pass params to shader)
        Terrain terrain = this.gameObject.GetComponent<Terrain>();

        // Pass updated light positions to shader
        terrain.materialTemplate.SetColor("_PointLightColor", this.pointLight.color);
        terrain.materialTemplate.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());

    }
    
}
