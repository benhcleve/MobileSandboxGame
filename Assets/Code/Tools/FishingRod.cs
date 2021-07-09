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
    bool canCast;

    private void OnEnable()
    {
        CameraManager.instance.state = CameraManager.State.Behind;
        CameraManager.instance.behindOffset = new Vector3(0.75f, 1.35f, 1.75f);
        UIManager.instance.uiState = UIManager.UIState.Default_NoMovement;
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
                castTarget.transform.position = hit.point + new Vector3(0, 1, 0);
                castTargetSprite.color = Color.green;
                canCast = true;
            }
        }
    }

    public void CastRod()
    {

    }
}
