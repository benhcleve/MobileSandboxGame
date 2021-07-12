using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class FishingLine : MonoBehaviour
{
    public Transform poleEnd;
    public Transform bobber;
    LineRenderer lineRenderer;
    public float tension;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, poleEnd.position);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, bobber.position);

        for (float i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            if (i > 0 && i < lineRenderer.positionCount)
            {
                Vector3 pointPos = Parabola(poleEnd.position, bobber.position, tension, (i / 10));
                lineRenderer.SetPosition((int)i, pointPos);
            }

        }
    }

}
