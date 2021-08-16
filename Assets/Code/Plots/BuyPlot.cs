using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPlot : Interactable
{
    public int buyCost;
    public GameObject buyUI;
    public Plot targetPlot;
    public override void Interact()
    {
        base.Interact();
        buyUI.SetActive(true);

    }

    public void BuyButton()
    {
        if (PlayerInventory.instance.coins >= buyCost)
        {
            targetPlot.isPurchased = true;
            PlotManager.instance.UpdatePlotNavAndFence();
            buyUI.SetActive(false);
            PlayerInventory.instance.coins -= buyCost;
        }
    }
}
