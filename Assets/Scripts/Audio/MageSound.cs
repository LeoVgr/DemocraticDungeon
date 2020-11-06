using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSound : MonoBehaviour
{
    [Header("Actions Sound Effects")]

    public AudioSource actionSounds;

    public AudioClip fireball;
    public AudioClip chloroquine;
    public AudioClip instantMemory;
    public AudioClip mortalTouch;

    [Header("Character Sound Effects")]

    public AudioSource characterSounds;

    public AudioClip[] effortSounds;
    public AudioClip[] hurtSounds;
    public AudioClip deathSound;
    public AudioClip victorySound;

    [Header("Mixer")]

    [Range(0f, 1f)]
    public float actionsVolume = 1f;
    [Range(0f, 1f)]
    public float characterVolume = 1f;

    private void Start()
    {
        characterSounds.playOnAwake = false;
        characterSounds.loop = false;
        actionSounds.playOnAwake = false;
        actionSounds.loop = false;
    }

    private void Update()
    {
        characterSounds.volume = characterVolume;
        actionSounds.volume = actionsVolume;
    }

    public void FireballSound()
    {
        EffortSound();
        actionSounds.clip = fireball;
        actionSounds.Play();
    }

    public void ChloroquineSound()
    {
        EffortSound();
        actionSounds.clip = chloroquine;
        actionSounds.Play();
    }

    public void InstantMemorySound()
    {
        EffortSound();
        actionSounds.clip = instantMemory;
        actionSounds.Play();
    }

    public void MortalTouchSound()
    {
        EffortSound();
        actionSounds.clip = mortalTouch;
        actionSounds.Play();
    }

    public void HurtSound()
    {
        characterSounds.clip = hurtSounds[UnityEngine.Random.Range(0, hurtSounds.Length)];
        characterSounds.Play();
    }

    public void EffortSound()
    {
        characterSounds.clip = effortSounds[UnityEngine.Random.Range(0, effortSounds.Length)];
        characterSounds.Play();
    }

    public void VictorySound()
    {
        characterSounds.clip = victorySound;
        characterSounds.Play();
    }

    public void DeathSound()
    {
        characterSounds.clip = deathSound;
        characterSounds.Play();
    }
}
