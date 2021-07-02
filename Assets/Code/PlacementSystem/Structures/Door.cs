using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;
    Transform player;
    public bool isPlaced;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }
        else
        {
            if (isPlaced)
            {
                if (Vector3.Distance(transform.position, player.position) < 2 && !animator.GetBool("isOpen"))
                    animator.SetBool("isOpen", true);
                if (Vector3.Distance(transform.position, player.position) >= 2 && animator.GetBool("isOpen"))
                    animator.SetBool("isOpen", false);
            }
        }

    }
}
