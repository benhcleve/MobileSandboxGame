using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassLighting : MonoBehaviour
{
    public Material grassMat;

    private void Update()
    {
        grassMat.SetColor("_LightColor", RenderSettings.ambientLight);
    }
}
