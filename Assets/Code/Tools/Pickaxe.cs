using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{

    public float damage = 5f;
    public GameObject pickaxeUI;
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
        if (PlayerInteraction.instance.target != null && PlayerInteraction.instance.target.GetComponent<OreBase>())
        {
            //if tree is within interaction range
            if (PlayerInteraction.instance.target.GetComponent<OreBase>().isInteracting && !pickaxeUI.activeInHierarchy)
                StartCoroutine(Mine());
        }
        if (PlayerInteraction.instance.target == null || !PlayerInteraction.instance.target.GetComponent<OreBase>())
            EndMining();
    }

    public void EndMining()
    {
        StopAllCoroutines();
        PlayerAnimation.instance.animator.SetBool("isChoppingHatchet", false);
        PlayerInteraction.instance.target = null;
        pickaxeUI.SetActive(false);
        UIManager.instance.uiState = UIManager.UIState.Default;
    }


    void MeterPress(float damageMult)
    {
        damageMultiplier = damageMult;
        slider.gameObject.SetActive(false);
    }

    IEnumerator Mine()
    {
        pickaxeUI.SetActive(true);
        PlayerAnimation.instance.animator.SetBool("isChoppingHatchet", true);
        UIManager.instance.uiState = UIManager.UIState.Minigame;

        while (true)
        {
            if (PlayerInteraction.instance.target == null) //End coroutine if tree is falling
                EndMining();

            timeElapsed = 0;
            hasTapped = false;
            slider.gameObject.SetActive(true);
            damageType = 0;

            while (timeElapsed <= lerpDuration)
            {

                slider.transform.position = Vector2.Lerp(sliderStart.position, sliderEnd.position, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;

                DetectHitMouse();
                DetectHitTouch();

                yield return new WaitForEndOfFrame();

            }
            if (timeElapsed >= lerpDuration && slider.gameObject.activeInHierarchy)
                MeterPress(Random.Range(0.3f, 0.5f));

            PlayerInteraction.instance.target.GetComponent<OreBase>().TakeDamage((int)(damage * damageMultiplier), damageType);

            //If animation state "Chop" is at least 90% complete, then move forward
            yield return new WaitUntil(() =>
            PlayerAnimation.instance.animator.GetCurrentAnimatorStateInfo(0).IsName("Chop") &&
            PlayerAnimation.instance.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .9f);
        }

    }
    void DetectHitTouch()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Minigame)
        {
            if (Input.touchCount > 0 && !hasTapped)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began) //Set starting position of touch 1
                {
                    PlayerInteraction.instance.target.GetComponent<ParticleSystem>().Play(); //Plays rock particles on hit

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
                    slider.gameObject.SetActive(false);
                    hasTapped = true;
                }

            }
        }
    }

    void DetectHitMouse()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Minigame)
        {
            if (Input.GetMouseButton(0) && !hasTapped)
            {
                PlayerInteraction.instance.target.GetComponent<ParticleSystem>().Play(); //Plays rock particles on hit

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
                slider.gameObject.SetActive(false);
                hasTapped = true;
            }
        }
    }


}
