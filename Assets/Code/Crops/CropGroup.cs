using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropGroup : MonoBehaviour
{
    public List<GameObject> grownCrops;
    private void Update()
    {
        for (int i = 0; i < grownCrops.Count; i++)
            if (grownCrops[i] == null)
                grownCrops.Remove(grownCrops[i]);
        if (grownCrops.Count == 0)
            Destroy(gameObject);
    }

}
