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
        {
            if (GameTime.instance.hour >= 20 || GameTime.instance.hour < 7) // If time is between 8PM and 7AM , get good rest
                energy += Time.deltaTime / 8;
            else energy += Time.deltaTime / 16; //Else rest is cut in half. Sleeping in day increases energy significantly slower to encourage good sleep cycle.
        }


        if (energy > 100)
            energy = 100;

        if (energy <= 0)
        {
            energy = 0;
            isTired = true;
        }
    }
}
