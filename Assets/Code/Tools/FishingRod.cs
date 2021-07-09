using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingRod : MonoBehaviour
{
    public FloatingJoystick turningJoy;
    public Slider powerSlider;
    public float castPower;
    private void OnEnable()
    {
        CameraManager.instance.state = CameraManager.State.Behind;
        CameraManager.instance.behindOffset = new Vector3(0.75f, 1.35f, 1.75f);
        UIManager.instance.uiState = UIManager.UIState.Default_NoMovement;
        powerSlider.maxValue = castPower; //Sets the power of the rod to the slider
    }

    private void OnDestroy()
    {
        CameraManager.instance.state = CameraManager.State.Default;
        UIManager.instance.uiState = UIManager.UIState.Default;
    }

    private void Update()
    {
        TurnPlayer();
    }

    void TurnPlayer()
    {
        PlayerMovement.instance.transform.Rotate(Vector3.up * turningJoy.Horizontal);
    }

    void HandlePower()
    {

    }
}
