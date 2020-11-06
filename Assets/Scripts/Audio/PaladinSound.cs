using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinSound : MonoBehaviour
{
    [Header("Actions Sound Effects")]

    public AudioSource actionSounds;

    public AudioClip healingLight;
    public AudioClip divineSmiteSuccess;
    public AudioClip divineSmiteFail;
    public AudioClip protectAlly;
    public AudioClip taunt;

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

    public void HealingLightSound()
    {
        EffortSound();
        actionSounds.clip = healingLight;
        actionSounds.Play();
    }

    public void DivineSmiteSuccessSound()
    {
        EffortSound();
        actionSounds.clip = divineSmiteSuccess;
        actionSounds.Play();
    }

    public void DivineSmiteFailSound()
    {
        EffortSound();
        actionSounds.clip = divineSmiteFail;
        actionSounds.Play();
    }

    public void ProtectAllySound()
    {
        EffortSound();
        actionSounds.clip = protectAlly;
        actionSounds.Play();
    }

    public void TauntSound()
    {
        EffortSound();
        actionSounds.clip = taunt;
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
