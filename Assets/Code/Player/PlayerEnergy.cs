using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public float energy = 100;
    public bool isTired;

    private static PlayerEnergy _instance;
    public static PlayerEnergy instance { get { return _instance; } }
    private void Awake() => CreateInstance();
    void CreateInstance() //Make this UI Manager an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    private void Update()
    {
        if (energy <= 100 && !PlayerAnimation.instance.animator.GetBool("isSleeping")) //If awake
            energy -= Time.deltaTime / 12;
        if (energy < 100 && PlayerAnimation.instance.animator.GetBool("isSleeping")) //If sleeping
            energy += Time.deltaTime / 8;

        if (energy > 100)
            energy = 100;

        if (energy <= 0)
        {
            energy = 0;
            isTired = true;
        }
    }
}
