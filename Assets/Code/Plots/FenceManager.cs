using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceManager : MonoBehaviour
{
    public Plot plot;
    public GameObject northFence;
    public GameObject southFence;
    public GameObject eastFence;
    public GameObject westFence;
    public GameObject prefabFence;
    public GameObject prefabFenceOpen;
    public GameObject prefabFenceBuyPlot;
    public bool openNorth;
    public bool openSouth;
    public bool openEast;
    public bool openWest;

    public void UpdateFencing()
    {
        GameObject[] allFencing = new GameObject[] { northFence, southFence, eastFence, westFence };

        foreach (GameObject fence in allFencing)
        {
            foreach (Transform child in fence.transform) //Destroy all children fences
                GameObject.Destroy(child.gameObject);

            Plot targetPlot = null;
            Transform parentFenceEmpty = null;

            //If this plot is purchased
            if (plot.isPurchased)
            {
                if (fence == northFence)
                {
                    parentFenceEmpty = northFence.transform;

                    if (PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(0, 1)) != null)
                        targetPlot = PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(0, 0)).GetComponent<Plot>();
                    else
                    {
                        GameObject newFence = Instantiate(prefabFence);
                        newFence.transform.parent = parentFenceEmpty;
                        newFence.transform.localPosition = Vector3.zero;
                        newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }

                }
                if (fence == southFence)
                {
                    parentFenceEmpty = southFence.transform;

                    if (PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(0, -1)) != null)
                        targetPlot = PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(0, -1)).GetComponent<Plot>();
                    else
                    {
                        GameObject newFence = Instantiate(prefabFence);
                        newFence.transform.parent = parentFenceEmpty;
                        newFence.transform.localPosition = Vector3.zero;
                        newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }

                }
                if (fence == eastFence)
                {
                    parentFenceEmpty = eastFence.transform;

                    if (PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(1, 0)) != null)
                        targetPlot = PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(1, 0)).GetComponent<Plot>();
                    else
                    {
                        GameObject newFence = Instantiate(prefabFence);
                        newFence.transform.parent = parentFenceEmpty;
                        newFence.transform.localPosition = Vector3.zero;
                        newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }

                }
                if (fence == westFence)
                {
                    parentFenceEmpty = westFence.transform;

                    if (PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(-1, 0)) != null)
                        targetPlot = PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(-1, 0)).GetComponent<Plot>();
                    else
                    {
                        GameObject newFence = Instantiate(prefabFence);
                        newFence.transform.parent = parentFenceEmpty;
                        newFence.transform.localPosition = Vector3.zero;
                        newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }

                }

                if (targetPlot != null)
                {
                    GameObject newFence = null;
                    if (targetPlot.isPurchased)
                        newFence = Instantiate(prefabFenceOpen);
                    if (!targetPlot.isPurchased)
                    {
                        newFence = Instantiate(prefabFenceBuyPlot);
                        newFence.transform.Find("Buy Land Sign").GetComponent<BuyPlot>().targetPlot = targetPlot; //Sets the plot sign to buy target plot
                    }


                    newFence.transform.parent = parentFenceEmpty;
                    newFence.transform.localPosition = Vector3.zero;
                    newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

            }
            //If this plot is NOT purchased
            if (!plot.isPurchased)
            {
                if (fence == northFence)
                {
                    parentFenceEmpty = northFence.transform;

                    if (PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(0, 1)) != null)
                        targetPlot = PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(0, 0)).GetComponent<Plot>();
                    else
                    {
                        GameObject newFence = Instantiate(prefabFence);
                        newFence.transform.parent = parentFenceEmpty;
                        newFence.transform.localPosition = Vector3.zero;
                        newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }

                }
                if (fence == southFence)
                {
                    parentFenceEmpty = southFence.transform;

                    if (PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(0, -1)) != null)
                        targetPlot = PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(0, -1)).GetComponent<Plot>();
                    else
                    {
                        GameObject newFence = Instantiate(prefabFence);
                        newFence.transform.parent = parentFenceEmpty;
                        newFence.transform.localPosition = Vector3.zero;
                        newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }

                }
                if (fence == eastFence)
                {
                    parentFenceEmpty = eastFence.transform;

                    if (PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(1, 0)) != null)
                        targetPlot = PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(1, 0)).GetComponent<Plot>();
                    else
                    {
                        GameObject newFence = Instantiate(prefabFence);
                        newFence.transform.parent = parentFenceEmpty;
                        newFence.transform.localPosition = Vector3.zero;
                        newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }

                }
                if (fence == westFence)
                {
                    parentFenceEmpty = westFence.transform;

                    if (PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(-1, 0)) != null)
                        targetPlot = PlotManager.instance.GetPlotByCoord(plot.plotCoordinates + new Vector2(-1, 0)).GetComponent<Plot>();
                    else
                    {
                        GameObject newFence = Instantiate(prefabFence);
                        newFence.transform.parent = parentFenceEmpty;
                        newFence.transform.localPosition = Vector3.zero;
                        newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }

                }

                if (targetPlot != null)
                {
                    GameObject newFence = null;
                    if (targetPlot.isPurchased)
                        newFence = null;
                    if (!targetPlot.isPurchased)
                        newFence = null;

                    if (newFence != null)
                    {
                        newFence.transform.parent = parentFenceEmpty;
                        newFence.transform.localPosition = Vector3.zero;
                        newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }
                }

            }
        }

        OpenFence();
    }


    void OpenFence()
    {


        if (openNorth)
        {

            foreach (Transform child in northFence.transform) //Destroy all children fences
                GameObject.Destroy(child.gameObject);

            GameObject newFence = Instantiate(prefabFenceOpen);
            newFence.transform.parent = northFence.transform;
            newFence.transform.localPosition = Vector3.zero;
            newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (openSouth)
        {
            foreach (Transform child in southFence.transform) //Destroy all children fences
                GameObject.Destroy(child.gameObject);

            GameObject newFence = Instantiate(prefabFenceOpen);
            newFence.transform.parent = southFence.transform;
            newFence.transform.localPosition = Vector3.zero;
            newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (openEast)
        {
            foreach (Transform child in eastFence.transform) //Destroy all children fences
                GameObject.Destroy(child.gameObject);

            GameObject newFence = Instantiate(prefabFenceOpen);
            newFence.transform.parent = eastFence.transform;
            newFence.transform.localPosition = Vector3.zero;
            newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (openWest)
        {
            foreach (Transform child in westFence.transform) //Destroy all children fences
                GameObject.Destroy(child.gameObject);

            GameObject newFence = Instantiate(prefabFenceOpen);
            newFence.transform.parent = westFence.transform;
            newFence.transform.localPosition = Vector3.zero;
            newFence.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }



}




