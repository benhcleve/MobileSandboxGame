using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FishingBobber : MonoBehaviour
{
    public bool isBeingNibbled;
    public GameObject hook;
    public GameObject bait;
    public GameObject hookedFish; //Fish that is hooked, if any
    LineRenderer lineRenderer;
    public ItemSlot baitSlot;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lineRenderer.SetPosition(1, hook.transform.localPosition);

        if (baitSlot.currentItem == null && bait != null)
        {
            Destroy(bait);
            bait = null;
        }
        if (baitSlot.currentItem != null && bait == null)
        {
            bait = Instantiate(baitSlot.currentItem.prefab, hook.transform.position, hook.transform.rotation);
            bait.transform.parent = hook.transform;
            bait.GetComponent<Rigidbody>().isKinematic = true;
            bait.GetComponent<Bait>().bobber = this;
            Destroy(bait.GetComponent<Pickupable>());
        }
    }

    public IEnumerator FishNibbleNotify()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            gameObject.transform.DOShakeScale(1, .5f, 10);
            gameObject.transform.DOPunchPosition((-transform.up * .1f), .5f, 1);
        }
    }
}
