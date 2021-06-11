using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TextPopup : MonoBehaviour
{
    RectTransform rect;
    public string popupText;
    public Color32 textColor;
    public Vector3 size;
    public enum Effect { BounceInFadeOut, EaseInFadeOut, EaseInEaseOut, Damage }
    public Effect effect;
    public float duration = 1f;
    public Transform target;
    public Vector3 targetOffset;

    void Start()
    {

        rect = GetComponent<RectTransform>();
        GetComponent<TextMeshProUGUI>().color = textColor;
        GetComponent<TextMeshProUGUI>().text = popupText;

        RunEffect();

    }

    private void Update()
    {
        if (target != null)
            PositionUI();
    }

    private void PositionUI()
    {
        // Calculate *screen* position (note, not a canvas/recttransform position)
        Vector2 canvasPos;
        RectTransform canvasRect = UIManager.instance.transform.GetComponent<RectTransform>();
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(target.position + targetOffset);
        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);
        // Set
        transform.localPosition = canvasPos;
    }

    void RunEffect()
    {
        switch (effect)
        {
            case Effect.BounceInFadeOut:
                StartCoroutine(BounceInFadeOut());
                break;
            case Effect.EaseInEaseOut:
                StartCoroutine(EaseInEaseOut());
                break;
            case Effect.Damage:
                StartCoroutine(Damage());
                break;

        }
        Destroy(gameObject, duration + 2);
    }

    IEnumerator BounceInFadeOut()
    {
        rect.localScale = Vector3.zero;
        rect.DOScale(size, 1f).SetEase(Ease.OutBounce);
        StartCoroutine(MoveOvertime(Vector3.up, .5f, 2));
        yield return new WaitForSeconds(duration);
        rect.DOScale(Vector3.zero, 1f).SetEase(Ease.Flash);
    }

    IEnumerator EaseInEaseOut()
    {
        rect.localScale = Vector3.zero;
        rect.DOScale(size, .3f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(duration);
        rect.DOScale(Vector3.zero, .3f).SetEase(Ease.OutQuad);
    }

    IEnumerator Damage()
    {
        rect.localScale = Vector3.zero;
        rect.DOScale(size, 1f).SetEase(Ease.OutBounce);
        StartCoroutine(MoveOvertime(Vector3.up, .5f, 1));
        yield return new WaitForSeconds(duration);
        rect.DOScale(Vector3.zero, .5f).SetEase(Ease.Flash);
    }


    IEnumerator MoveOvertime(Vector3 direction, float speed, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            targetOffset += (direction * speed) * Time.deltaTime;
            duration += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
