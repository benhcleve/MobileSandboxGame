using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hatchet : MonoBehaviour
{
    public float damage = 5f;
    public GameObject hatchetUI;
    float damageMultiplier;
    int damageType; //0 idle, 1 weak, 2 hit, 3 critical


    //Meter Props
    bool hasTapped;
    public HitSlider slider;
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
        if (PlayerInteraction.instance.target != null && PlayerInteraction.instance.target.GetComponent<TreeBase>())
        {
            //if tree is within interaction range
            if (PlayerInteraction.instance.target.GetComponent<TreeBase>().isInteracting && !hatchetUI.activeInHierarchy)
                StartCoroutine(Chop());

        }
        if (PlayerInteraction.instance.target == null && UIManager.instance.uiState == UIManager.UIState.Minigame)
            EndChopping();
    }

    public void EndChopping()
    {
        StopAllCoroutines();
        UIManager.instance.uiState = UIManager.UIState.Default;
        PlayerAnimation.instance.animator.SetBool("isChoppingHatchet", false);
        PlayerInteraction.instance.target = null;

        hatchetUI.SetActive(false);
    }


    void MeterPress(float damageMult)
    {
        if (hasTapped)
        {
            damageMultiplier = damageMult;
            slider.gameObject.SetActive(false);
        }
    }

    IEnumerator Chop()
    {

        UIManager.instance.uiState = UIManager.UIState.Minigame;
        hatchetUI.SetActive(true);

        yield return new WaitForSeconds(.1f); //Wait to prevent click of object to not count as a meter press

        while (true)
        {
            if (PlayerInteraction.instance.target == null || !PlayerInteraction.instance.target.GetComponent<TreeBase>() || PlayerInteraction.instance.target.GetComponent<TreeBase>().fallen) //End coroutine if tree is falling
            {
                EndChopping();
                break;
            }


            PlayerAnimation.instance.animator.SetBool("isChoppingHatchet", true);
            timeElapsed = 0;
            hasTapped = false;
            slider.gameObject.SetActive(true);
            damageType = 0;

            while (timeElapsed <= lerpDuration)
            {

                slider.transform.position = Vector2.Lerp(sliderStart.position, sliderEnd.position, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;

                DetectChopMouse();
                DetectChopTouch();

                yield return new WaitForEndOfFrame();

            }
            if (timeElapsed >= lerpDuration && slider.gameObject.activeInHierarchy)
                MeterPress(Random.Range(0.3f, 0.5f));

            PlayerInteraction.instance.target.GetComponent<TreeBase>().TakeDamage((int)(damage * damageMultiplier), damageType);

            //If animation state "Chop" is at least 90% complete, then move forward
            yield return new WaitUntil(() =>
            PlayerAnimation.instance.animator.GetCurrentAnimatorStateInfo(0).IsName("Chop") &&
            PlayerAnimation.instance.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .9f);


        }

    }
    void DetectChopTouch()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Minigame)
        {
            if (Input.touchCount > 0 && !hasTapped)
            {
                hasTapped = true;
                if (Input.GetTouch(0).phase == TouchPhase.Began) //Set starting position of touch 1
                {

                    switch (slider.state)
                    {
                        case HitSlider.State.WeakHitZone:
                            MeterPress(Random.Range(0.6f, 1f));
                            damageType = 1;
                            break;
                        case HitSlider.State.HitZone:
                            MeterPress(Random.Range(1.1f, 1.5f));
                            damageType = 2;
                            break;
                        case HitSlider.State.CriticalZone:
                            MeterPress(Random.Range(2f, 2.5f));
                            damageType = 3;
                            break;
                    }

                }

            }
        }
    }

    void DetectChopMouse()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Minigame)
        {
            if (Input.GetMouseButton(0) && !hasTapped)
            {
                hasTapped = true;

                switch (slider.state)
                {
                    case HitSlider.State.WeakHitZone:
                        MeterPress(Random.Range(0.6f, 1f));
                        damageType = 1;
                        break;
                    case HitSlider.State.HitZone:
                        MeterPress(Random.Range(1.1f, 1.5f));
                        damageType = 2;
                        break;
                    case HitSlider.State.CriticalZone:
                        MeterPress(Random.Range(2f, 2.5f));
                        damageType = 3;
                        break;
                }

            }
        }
    }



}

