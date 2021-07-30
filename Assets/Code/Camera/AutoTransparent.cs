using UnityEngine;
using System.Collections;


public class AutoTransparent : MonoBehaviour
{
    public Material ditherMat;
    public Material originalMat;
    private Renderer myRenderer;
    LayerMask originalLayer;
    public bool isTransparent;
    int materialIndex;

    private void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
        originalMat = myRenderer.material;
        originalLayer = gameObject.layer;
    }


    public void BeTransparent()
    {
        myRenderer = GetComponent<MeshRenderer>();

        if (myRenderer.material != ditherMat)
        {
            myRenderer.material = ditherMat;
            if (originalMat.GetTexture("_MainTex"))
                myRenderer.material.SetTexture("_MainTex", originalMat.GetTexture("_MainTex"));
            if (originalMat.GetColor("_MainColor") != null)
                myRenderer.material.SetColor("_MainColor", originalMat.GetColor("_MainColor"));
            myRenderer.gameObject.layer = LayerMask.NameToLayer("Dithered");
        }
        isTransparent = true;
    }
    void Update()
    {
        if (!isTransparent)
        {
            // Remove the dither
            myRenderer.material = originalMat;
            myRenderer.gameObject.layer = originalLayer;
            // And remove this script
            Destroy(this);
        }
    }
    private void LateUpdate() => isTransparent = false;



}