using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    public GameObject[] Plots;
    public GameObject activePlot;
    private static PlotManager _instance;
    public static PlotManager instance { get { return _instance; } }

    public void Awake() => CreateInstance();
    void CreateInstance() //Make this an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }


    private void Start()
    {
        foreach (GameObject plot in Plots)
        {
            plot.GetComponent<Plot>().BakeTerrain();
        }
    }

    private void Update()
    {
        SetActivePlot();
    }
    public void SetActivePlot()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(PlayerMovement.instance.transform.position + new Vector3(0, 1, 0), Vector3.down, 10);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.GetComponent<Terrain>() && hits[i].transform.parent.gameObject.GetComponent<Plot>())
            {
                activePlot = hits[i].transform.parent.gameObject;
            }
        }
    }

    public GameObject GetPlotByCoord(Vector2 coord)
    {
        foreach (GameObject plot in Plots)
        {
            if (plot.GetComponent<Plot>().plotCoordinates == coord)
                return plot;
        }
        return null;
    }
}
