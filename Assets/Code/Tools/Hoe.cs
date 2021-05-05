using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoe : MonoBehaviour
{
    public GameObject tilledSoil;
    private void OnEnable()
    {
        ObjectPlacement.instance.gameObject.SetActive(true);
        ObjectPlacement.instance.placedObjectPrefab = tilledSoil;
        ObjectPlacement.instance.buildTimer = 5;
        PlayerAnimation.instance.SetBuildAnimation(1);
    }

    private void OnDestroy()
    {
        ObjectPlacement.instance.placedObjectPrefab = null;
        ObjectPlacement.instance.gameObject.SetActive(false);
    }
}
