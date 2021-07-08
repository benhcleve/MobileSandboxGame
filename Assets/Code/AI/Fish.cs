using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public WaterBody waterBody;
    public float speed = 1;
    Vector3 navPoint;
    public bool showDebug;

    void Start()
    {
        StartCoroutine(FishWander());
        navPoint = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, navPoint, speed * Time.deltaTime);
        transform.LookAt(navPoint, Vector3.up);
    }

    IEnumerator FishWander()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 10f));
            int navRod = Random.Range(0, waterBody.groundPoints.Count);
            float depth = Random.Range(0.0f, 1.0f);
            Vector3 point = Vector3.Lerp(waterBody.groundPoints[navRod], (waterBody.surfacePoints[navRod] + waterBody.transform.position), depth);
            navPoint = point;

            speed = Random.Range(0.1f, 0.5f);

        }
    }

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            if (navPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(navPoint, 0.1f);
            }
        }
    }



}
