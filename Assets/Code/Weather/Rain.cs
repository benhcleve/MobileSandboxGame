using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    public GameObject rainSplash;
    public int rainIntensity = 0;

    ParticleSystem part;
    List<ParticleCollisionEvent> collisionEvents;

    private void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void Update()
    {
        var emission = part.emission;
        if (part.emission.rateOverTime.constant != rainIntensity * 100)
            emission.rateOverTime = rainIntensity * 100;
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        int i = 0;

        while (i < numCollisionEvents)
        {
            Vector3 pos = collisionEvents[i].intersection;
            GameObject splash = Instantiate(rainSplash, pos, Quaternion.identity);
            splash.transform.parent = transform;
            i++;
        }
    }


}
