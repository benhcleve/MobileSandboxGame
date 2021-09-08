using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    enum State { Idle, Fly, Eat };
    State state = State.Idle;
    Animator animator;
    Vector3 flightDestination;
    Transform idleDestination;
    public float flightSpeed;

    AudioSource audioSource;
    public AudioClip[] chirps;


    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ChangeFlightDestination());
        StartCoroutine(ChangeState());
        StartCoroutine(Chirp());
    }

    void Update()
    {
        ManageState();
    }

    void ManageState()
    {
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Fly:
                Fly();
                break;
            case State.Eat:
                Eat();
                break;
        }

        if (state != State.Idle && idleDestination != null)
            idleDestination = null;


    }

    IEnumerator Chirp()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));
            audioSource.pitch = Random.Range(0.5f, 1f);
            audioSource.PlayOneShot(chirps[Random.Range(0, chirps.Length - 1)]);
        }
    }

    IEnumerator ChangeFlightDestination()
    {
        while (true)
        {
            flightDestination = new Vector3(Random.Range(-600, 600), Random.Range(10, 30), Random.Range(-600, 600));
            yield return new WaitForSeconds(Random.Range(3, 20));
        }
    }

    IEnumerator ChangeState()
    {
        while (true)
        {
            state = (State)Random.Range(0, 2);
            yield return new WaitForSeconds(Random.Range(10, 20));
        }
    }

    void Idle()
    {
        if (idleDestination == null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 30);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.name == "BirdSpot")
                {
                    idleDestination = hitCollider.transform;
                    break;
                }
                if (hitCollider == hitColliders[hitColliders.Length - 1]) //If no idle spots around, return to fly state
                    state = State.Fly;
            }
        }
        if (idleDestination != null)
        {
            if (Vector3.Distance(transform.position, idleDestination.position) > .2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, idleDestination.position, flightSpeed * Time.deltaTime);
                transform.LookAt(idleDestination);
                if (!animator.GetBool("isFlying"))
                    SetAnimation("isFlying");
            }
            else if (Vector3.Distance(transform.position, idleDestination.position) <= .2f)
            {
                if (!animator.GetBool("isIdle"))
                    SetAnimation("isIdle");

            }
        }
    }

    void Fly()
    {
        //If bird is below 10, fly upward
        if (transform.position.y < 10)
            transform.Translate(0, 1 * Time.deltaTime, 0);

        //Change destination if within 1 unit of distance
        if (Vector3.Distance(transform.position, flightDestination) < 1)
            flightDestination = new Vector3(Random.Range(-600, 600), Random.Range(10, 30), Random.Range(-600, 600));

        if (!animator.GetBool("isFlying"))
            SetAnimation("isFlying");

        transform.position = Vector3.MoveTowards(transform.position, flightDestination, flightSpeed * Time.deltaTime);
        transform.LookAt(flightDestination);
    }

    void Eat()
    {

    }

    void SetAnimation(string name)
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isFlying", false);
        animator.SetBool("isEating", false);

        animator.SetBool(name, true);
    }

}
