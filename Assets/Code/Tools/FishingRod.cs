using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    private void OnEnable()
    {
        CameraManager.instance.state = CameraManager.State.Behind;
        CameraManager.instance.behindOffset = new Vector3(0.75f, 1.35f, 1.75f);
        UIManager.instance.uiState = UIManager.UIState.Default_NoMovement;
    }

    private void OnDestroy()
    {
        CameraManager.instance.state = CameraManager.State.Default;
        UIManager.instance.uiState = UIManager.UIState.Default;
    }
}
