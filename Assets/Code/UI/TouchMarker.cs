﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchMarker : MonoBehaviour
{
    public GameObject player;
    public GameObject moveMarker;
    public Image targetMarker;
    public Image screenPanel;

    void Update()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Default)
        {
            if (player.GetComponent<PlayerInteraction>().target != null)
            {
                targetMarker.gameObject.SetActive(true);
                UpdateTargetMarker();
                moveMarker.SetActive(false);
                transform.position = player.GetComponent<PlayerInteraction>().target.transform.position;
            }
            else if (player.GetComponent<PlayerInteraction>().target == null && PlayerMovement.instance.navMeshAgent.hasPath)
            {
                moveMarker.SetActive(true);
                targetMarker.gameObject.SetActive(false);
            }
            else
            {
                moveMarker.SetActive(false);
                targetMarker.gameObject.SetActive(false);
            }
        }
        else
        {
            moveMarker.SetActive(false);
            targetMarker.gameObject.SetActive(false);
        }
    }

    void UpdateTargetMarker()
    {
        GameObject target = player.GetComponent<PlayerInteraction>().target;

        // Calculate *screen* position (note, not a canvas/recttransform position)
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(target.transform.position);

        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(screenPanel.rectTransform, screenPoint, null, out canvasPos);

        // Set
        targetMarker.transform.localPosition = canvasPos;

    }


}
