using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    private static PlayerAnimation _instance;
    public static PlayerAnimation instance { get { return _instance; } }

    public void Awake() => CreateInstance();
    void CreateInstance() //Make this an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

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
        if (!animator.GetBool("isBuilding") && !animator.GetBool("isFishing") && !animator.GetBool("isMiningOre") && !animator.GetBool("isChoppingHatchet"))
        {
            if (GetComponent<PlayerMovement>().navMeshAgent.velocity != Vector3.zero && !animator.GetBool("isRunning"))
                SetAnimation("isRunning");
            else if (GetComponent<PlayerMovement>().navMeshAgent.velocity == Vector3.zero && !animator.GetBool("isIdle"))
                SetAnimation("isIdle");
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);
        }
    }

    public void SetBuildAnimation(int animID) => animator.SetInteger("buildAnim", animID);

    public void SetAnimation(string animName = null)
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdle", false);


        //Add any base animations here:
        string[] baseLayerBools = new string[] { "isRunning", "isIdle", "isBuilding" };

        //Set all animations false
        foreach (string animBoolName in baseLayerBools)
            animator.SetBool(animBoolName, false);

        if (animName != null) //Set desired animation true if not null
            animator.SetBool(animName, true);

    }
}
