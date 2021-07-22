using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSlider : MonoBehaviour
{
    public enum State { WeakHitZone, HitZone, CriticalZone }
    public State state = State.WeakHitZone;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.name == "Meter")
            state = State.WeakHitZone;
        if (other.transform.name == "HitZone")
            state = State.HitZone;
        if (other.transform.name == "CriticalZone")
            state = State.CriticalZone;
    }


}
