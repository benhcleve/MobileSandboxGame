using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingRod : MonoBehaviour
{
    public FloatingJoystick turningJoy;
    public Slider powerSlider;
    public float castPower;
    public GameObject castTarget;
    public SpriteRenderer castTargetSprite;
    public LayerMask waterLayer;
    public GameObject bobber;
    FishingLine fishingLine;
    bool canCast;
    public GameObject castUI;

    private void OnEnable()
    {
        CameraManager.instance.state = CameraManager.State.Behind;
        CameraManager.instance.behindOffset = new Vector3(0.75f, 1.35f, 1.75f);
        UIManager.instance.uiState = UIManager.UIState.Default_NoMovement;
        fishingLine = GetComponent<FishingLine>();
        powerSlider.maxValue = castPower; //Sets the power of the rod to the slider
        castTarget.SetActive(true);
    }

    private void OnDestroy()
    {
        Destroy(castTarget);
        CameraManager.instance.state = CameraManager.State.Default;
        UIManager.instance.uiState = UIManager.UIState.Default;
    }

    private void Update()
    {
        TurnPlayer();
        HandlePower();
    }

    void TurnPlayer()
    {
        PlayerMovement.instance.transform.Rotate(Vector3.up * turningJoy.Horizontal);
    }

    void HandlePower()
    {
        if (castTarget.activeInHierarchy)
        {
            if (castTarget.transform.parent == this.transform)
                castTarget.transform.parent = null;

            castTarget.transform.position = PlayerMovement.instance.transform.position + (PlayerMovement.instance.transform.forward * (powerSlider.value + 1));
            castTarget.transform.rotation = PlayerMovement.instance.transform.rotation;
            castTargetSprite.color = Color.red;
            canCast = false;

            RaycastHit hit;
            if (Physics.Raycast(castTarget.transform.position, -castTarget.transform.up, out hit, 100f, waterLayer))
            {
                castTarget.transform.position = hit.point + new Vector3(0, 0.2f, 0);
                castTargetSprite.color = Color.green;
                canCast = true;
            }
        }
    }

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    public void CastRod()
    {
        if (canCast)
        {
            castUI.SetActive(false);
            bobber.transform.parent = null;
            bobber.transform.eulerAngles = Vector3.zero;
            StartCoroutine(CastRodCo());
        }
    }

    IEnumerator CastRodCo()
    {
        float castTime = 0;
        Vector3 targetPos = castTarget.transform.position;
        castTarget.SetActive(false);

        while (castTime < 1f)
        {
            castTime += Time.deltaTime;
            bobber.transform.position = Parabola(fishingLine.poleEnd.position, targetPos, 1, castTime);
            yield return new WaitForEndOfFrame();
        }
        fishingLine.tension = -.1f;

    }
}
