using UnityEngine;
using System.Collections;


public class ClearSight : MonoBehaviour
{
    public float DistanceToPlayer;
    public Material ditherMat;
    public LayerMask ditherLayers;

    void Update()
    {
        DistanceToPlayer = Vector3.Distance(transform.position, PlayerMovement.instance.transform.position) - 3;

        RaycastHit[] hits;
        // you can also use CapsuleCastAll()
        // TODO: setup your layermask it improve performance and filter your hits.
        hits = Physics.SphereCastAll(transform.position, 1f, transform.forward, DistanceToPlayer, ditherLayers);
        foreach (RaycastHit hit in hits)
        {
            Renderer R = hit.collider.gameObject.GetComponent<MeshRenderer>();
            if (R == null)
                continue; // no renderer attached? go to next hit
                          // TODO: maybe implement here a check for GOs that should not be affected like the player


            AutoTransparent AT = R.GetComponent<AutoTransparent>();
            if (AT == null) // if no script is attached, attach one
            {
                AT = R.gameObject.AddComponent<AutoTransparent>();
            }
            AT.BeTransparent(); // get called every frame to reset the falloff
            if (AT.ditherMat != ditherMat)
                AT.ditherMat = ditherMat; //Set dither Mat on script
        }
    }

}