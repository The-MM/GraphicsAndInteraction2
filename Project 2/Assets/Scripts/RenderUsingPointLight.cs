using UnityEngine;
using System.Collections;

public class RenderUsingPointLight : MonoBehaviour
{
    public GameObject pointLightSource;

    // Called each frame
    void Update()
    {
        // Get renderer component (in order to pass params to shader)
        Renderer renderer = this.gameObject.GetComponent<Renderer>();

        // Pass updated light positions to shader
        renderer.material.SetColor("_PointLightColor", this.pointLightSource.GetComponent<PointLight>().color);
        renderer.material.SetVector("_PointLightPosition", this.pointLightSource.GetComponent<PointLight>().GetWorldPosition());
    }
    
}
