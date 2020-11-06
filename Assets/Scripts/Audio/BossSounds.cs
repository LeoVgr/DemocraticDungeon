using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSounds : MonoBehaviour
{
    [Header("Actions Sound Effects")]

    public AudioSource actionSounds;

    public AudioClip shrillCry;
    public AudioClip stomp;
    public AudioClip rainOfFeathers;
    public AudioClip arrogantBird;

    [Header("Character Sound Effects")]

    public AudioSource characterSounds;

    public AudioClip[] effortSounds;
    public AudioClip[] hurtSounds;
    public AudioClip deathSound;
    public AudioClip victorySound;
    public AudioClip looseSound;

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

    public void ArrogantBirdSound()
    {
        EffortSound();
        actionSounds.clip = arrogantBird;
        actionSounds.Play();
    }

    public void ShrillCrySound()
    {
        EffortSound();
        actionSounds.clip = shrillCry;
        actionSounds.Play();
    }

    public void StompSound()
    {
        EffortSound();
        actionSounds.clip = stomp;
        actionSounds.Play();
    }

    public void TauntSound()
    {
        EffortSound();
        actionSounds.clip = rainOfFeathers;
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

    public void LooseSound()
    {
        characterSounds.clip = looseSound;
        characterSounds.Play();
    }

    public void DeathSound()
    {
        characterSounds.clip = deathSound;
        characterSounds.Play();
    }
}
