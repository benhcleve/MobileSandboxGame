using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "Scriptables/LightingPreset", order = 10)]

public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient SkyColor;

}
