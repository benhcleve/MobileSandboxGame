using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPlot : Interactable
{
    public int buyCost;
    public GameObject buyCanvas;
    public GameObject buyUI;
    public GameObject notEnoughCoinUI;
    public Plot targetPlot;
    public override void Interact()
    {
        base.Interact();
        buyCanvas.SetActive(true);

    }

    public void BuyButton()
    {
        if (PlayerInventory.instance.coins >= buyCost)
        {
            targetPlot.isPurchased = true;
            PlotManager.instance.UpdatePlotNavAndFence();
            buyCanvas.SetActive(false);
            PlayerInventory.instance.coins -= buyCost;
        }
        else
        {
            buyUI.SetActive(false);
            notEnoughCoinUI.SetActive(true);
        }
    }
}
