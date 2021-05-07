using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    bool DirectionalLightLerping = false;
    public float hour;


    private void Update()
    {
        hour = (float)GameTime.instance.hour + ((float)GameTime.instance.minute / 60);
        var timePercent = hour / 24;
        UpdateLighting(timePercent);
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);


        DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
        var newDirectionalLightRot = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        if (!DirectionalLightLerping)
            StartCoroutine(LerpDirectionalLight(DirectionalLight.transform.localRotation, newDirectionalLightRot, 1));
    }

    IEnumerator LerpDirectionalLight(Quaternion fromRot, Quaternion toRot, float timeToMove)
    {
        DirectionalLightLerping = true;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            DirectionalLight.transform.localRotation = Quaternion.Lerp(fromRot, toRot, t);
            yield return null;
        }
        DirectionalLightLerping = false;
    }

}
