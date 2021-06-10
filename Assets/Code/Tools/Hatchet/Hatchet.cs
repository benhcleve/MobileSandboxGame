using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hatchet : MonoBehaviour
{
    public float damage = 5f;
    public GameObject hatchetUI;
    float damageMultiplier;

    //Meter Props
    bool hasTapped;
    public HatchetSlider slider;
    public Transform sliderStart;
    public Transform sliderEnd;
    public float lerpDuration = 1;
    public float timeElapsed = 0;

    void Update()
    {
        EnableDisableChopUI();
    }

    void EnableDisableChopUI()
    {
        if (PlayerInteraction.instance.target != null && PlayerInteraction.instance.target.tag == "Tree")
        {
            //if tree is within interaction range
            if (PlayerInteraction.instance.target.GetComponent<TreeBase>().isInteracting && !hatchetUI.activeInHierarchy)
                StartCoroutine(Lerp());
        }
        if (PlayerInteraction.instance.target == null || PlayerInteraction.instance.target.tag != "Tree")
            EndChopping();
    }

    public void EndChopping()
    {
        StopAllCoroutines();
        PlayerAnimation.instance.animator.SetBool("isChoppingHatchet", false);
        PlayerInteraction.instance.target = null;
        hatchetUI.SetActive(false);
        UIManager.instance.uiState = UIManager.UIState.Default;
    }

    void DetectChopTouch()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Minigame)
        {
            if (Input.touchCount > 0 && !hasTapped)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began) //Set starting position of touch 1
                {
                    switch (slider.state)
                    {
                        case HatchetSlider.State.WeakHitZone:
                            MeterPress(Random.Range(0.6f, 1f));
                            break;
                        case HatchetSlider.State.HitZone:
                            MeterPress(Random.Range(1.1f, 1.5f));
                            break;
                        case HatchetSlider.State.CriticalZone:
                            MeterPress(Random.Range(2f, 2.5f));
                            break;
                    }
                    slider.gameObject.SetActive(false);
                }
                hasTapped = true;
            }
        }
    }

    void MeterPress(float damageMult)
    {
        Debug.Log("Called Meter Press");
        damageMultiplier = damageMult;
        Debug.Log(Mathf.RoundToInt(damage * damageMultiplier));
        slider.gameObject.SetActive(false);
    }

    IEnumerator Lerp()
    {
        hatchetUI.SetActive(true);
        PlayerAnimation.instance.animator.SetBool("isChoppingHatchet", true);
        UIManager.instance.uiState = UIManager.UIState.Minigame;

        while (true)
        {

            timeElapsed = 0;
            hasTapped = false;
            slider.gameObject.SetActive(true);


            while (timeElapsed <= lerpDuration)
            {

                slider.transform.position = Vector2.Lerp(sliderStart.position, sliderEnd.position, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;

                DetectChopTouch();
                yield return new WaitForEndOfFrame();

            }
            if (timeElapsed >= lerpDuration && slider.gameObject.activeInHierarchy)
                MeterPress(Random.Range(0.3f, 0.5f));

            //If animation state "Chop" is at least 90% complete, then move forward
            yield return new WaitUntil(() =>
            PlayerAnimation.instance.animator.GetCurrentAnimatorStateInfo(0).IsName("Chop") &&
            PlayerAnimation.instance.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .9f);


        }



    }



}

