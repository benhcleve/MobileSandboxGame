using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public WaterBody waterBody;
    public float speed = 1;
    Vector3 navPoint;
    public bool showDebug;
    public GameObject bait;
    public bool isHooked;

    void Start()
    {
        StartCoroutine(FishWander());
        navPoint = transform.position;

    }

    void Update()
    {
        AttractToBait();
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

    public void AttractToBait()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + (transform.forward * .5f), 1);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.transform.GetComponent<Bait>() && !hitCollider.transform.GetComponent<Bait>().beingNibbled)
            {
                navPoint = hitCollider.transform.position + (-hitCollider.transform.up * .25f);

                if (Vector3.Distance(transform.position, navPoint) < .2f && bait != hitCollider.gameObject && !isHooked)
                {
                    StopAllCoroutines();
                    navPoint = transform.position;
                    bait = hitCollider.gameObject;
                    bait.GetComponent<Bait>().ReactToNibble(gameObject);
                }
            }
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
