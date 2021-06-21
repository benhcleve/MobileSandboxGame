using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
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


        // DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

    }



}
