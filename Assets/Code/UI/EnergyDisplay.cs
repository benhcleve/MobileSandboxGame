using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour
{

    public Image energyFillBar;
    void Update()
    {
        //Bar fill amount
        energyFillBar.fillAmount = PlayerEnergy.instance.energy / 100;

        //Change bar color based on energy level
        if (PlayerEnergy.instance.energy < 30 && PlayerEnergy.instance.energy >= 10)
            energyFillBar.color = Color.yellow;
        else if (PlayerEnergy.instance.energy < 10)
            energyFillBar.color = Color.red;
        else energyFillBar.color = Color.green;
    }
}
