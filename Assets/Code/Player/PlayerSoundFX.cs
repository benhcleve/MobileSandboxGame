using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundFX : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip footStepSFX;
    public AudioClip woodChopSFX;
    public AudioClip rockMineSFX;
    public ParticleSystem footstepParticles;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Step()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(footStepSFX);
        footstepParticles.Play();
    }

    private void WoodChop()
    {
        audioSource.pitch = Random.Range(1f, 1.5f);
        audioSource.PlayOneShot(woodChopSFX);
    }

    private void RockMine()
    {
        audioSource.pitch = Random.Range(1f, 1.5f);
        audioSource.PlayOneShot(rockMineSFX);
    }


}
