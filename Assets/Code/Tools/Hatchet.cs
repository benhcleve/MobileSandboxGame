using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatchet : MonoBehaviour
{
    bool isChopping;

    void Update()
    {


        if (PlayerInteraction.instance.target != null && PlayerInteraction.instance.target.tag == "Tree")
        {
            GameObject targetTree = PlayerInteraction.instance.target;

            //if tree is within interaction range
            if (targetTree.GetComponent<TreeBase>().isInteracting && !isChopping)
            {
                isChopping = true;
                Debug.Log("YTEEEYTY");
            }
        }
        else if (PlayerInteraction.instance.target == null || PlayerInteraction.instance.target.tag != "Tree")
        {
            if (isChopping)
            {
                isChopping = false;
            }
        }

    }


}
