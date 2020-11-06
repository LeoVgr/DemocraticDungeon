using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSound : MonoBehaviour
{
    [Header("Actions Sound Effects")]

    public AudioSource actionSounds;

    public List<AudioClip> skillSounds;

    [Header("Character Sound Effects")]

    public AudioSource characterSounds;

    public bool effortEnabled = true;
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

    public void SkillSound(int index)
    {
        EffortSound();
        actionSounds.clip = skillSounds[index];
        actionSounds.Play();
    }
    public void HurtSound()
    {
        characterSounds.clip = hurtSounds[UnityEngine.Random.Range(0, hurtSounds.Length)];
        characterSounds.Play();
    }

    public void EffortSound()
    {
        if (effortEnabled)
        {
            characterSounds.clip = effortSounds[UnityEngine.Random.Range(0, effortSounds.Length)];
            characterSounds.Play();
        }
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
