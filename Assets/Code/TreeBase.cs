using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBase : Interactable
{
    public int hitpoints;
    public GameObject treeSeed;
    public GameObject woodDrop;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetInteractionFalse();

    }
}
