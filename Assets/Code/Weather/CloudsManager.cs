using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsManager : MonoBehaviour
{
    public ParticleSystem cloudParticles;
    void Start() => cloudParticles = GetComponent<ParticleSystem>();

    public void UpdateClouds(int count, float interval, int size, Color32 color)
    {
        var _main = cloudParticles.main;
        var _emission = cloudParticles.emission;
        _emission.SetBurst(0, new ParticleSystem.Burst(0.0f, (short)count, _emission.GetBurst(0).cycleCount, interval));
        _main.startSize = size;

        cloudParticles.GetComponent<Renderer>().material.SetColor("_MainColor", color);

    }
}
