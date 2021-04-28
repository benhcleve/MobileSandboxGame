using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementAnimation();
    }

    void HandleMovementAnimation()
    {
        if (GetComponent<PlayerMovement>().navMeshAgent.velocity != Vector3.zero)
            SetAnimation("isWalking");
        else SetAnimation("isIdle");

    }



    void SetAnimation(string animName)
    {
        //Set all animations false
        foreach (AnimatorControllerParameter parameter in animator.parameters)
            animator.SetBool(parameter.name, false);

        //Set desired animation true
        animator.SetBool(animName, true);

    }
}
