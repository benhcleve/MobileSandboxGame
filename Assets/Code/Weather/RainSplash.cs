using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RainSplash : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SplashCo());
    }

    IEnumerator SplashCo()
    {
        transform.DOScale(Vector3.one / 5, .1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOScale(new Vector3(transform.localScale.x, 0, transform.localScale.z), .1f);
        yield return new WaitForSeconds(0.1f);
        transform.DOKill();
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.LookAt(PlayerMovement.instance.cam.transform);


    }

}
