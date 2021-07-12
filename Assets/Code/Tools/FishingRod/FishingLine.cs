using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class FishingLine : MonoBehaviour
{
    public FishingRod fishingRod;
    public Transform poleEnd;
    public FishingBobber bobber;
    LineRenderer lineRenderer;
    public float tension;

    private void Start()
    {
        fishingRod = GetComponent<FishingRod>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        LineTension();
        PositionLines();
    }

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    public void PositionLines()
    {
        lineRenderer.SetPosition(0, poleEnd.position);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, bobber.transform.position);

        for (float i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            if (i > 0 && i < lineRenderer.positionCount)
            {
                Vector3 pointPos;
                if (tension < 0f)
                    pointPos = Parabola(poleEnd.position, bobber.transform.position, tension, (i / 10)); //Arc line if tension is less than 0
                else pointPos = Parabola(poleEnd.position, bobber.transform.position, 0, (i / 10));

                lineRenderer.SetPosition((int)i, pointPos);
            }
        }
    }

    public void LineTension()
    {
        if (bobber.transform.parent != fishingRod.transform) //If casted (Bobber unparented when cast)
        {
            //If reeling in, increase tension to fishing rod reel speed
            if (fishingRod.isReeling && tension < fishingRod.reelSpeed)
            {
                tension += Time.deltaTime * fishingRod.reelSpeed;
            }
            //If no fish hooked and not reeling in, naturally loosen tension
            else if (!fishingRod.isReeling && bobber.hookedFish == null && tension > -0.3)
            {
                if (tension > 0)
                    tension = 0;

                tension -= Time.deltaTime / 5;
            }
        }
    }



}
