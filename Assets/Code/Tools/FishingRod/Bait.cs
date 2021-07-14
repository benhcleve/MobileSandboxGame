using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bait : MonoBehaviour
{
    public FishingBobber bobber;
    public bool beingNibbled;

    public void ReactToNibble(GameObject fish)
    {
        bobber.hookedFish = fish;
        beingNibbled = true;
        StartCoroutine(bobber.FishNibbleNotify());
    }
}
