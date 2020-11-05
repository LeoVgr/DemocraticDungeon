using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer master;

    [Header("Music")]

    public AudioSource epicMusicSource;
    public AudioClip epicMusic;

    public AudioSource bossaMusicSource;
    public AudioClip bossaMusic;

    public float crossfadeFactor;

    [Header("Ambience")]

    public AudioSource ambSource;
    public AudioClip bed;
    public AudioClip[] emitters;

    [Header("UI")]

    public AudioSource interfaceSounds;
    public AudioClip timer;
    public AudioClip gameOverSound;
    public AudioClip winGameSound;
    public AudioClip startGameSound;


    // Start is called before the first frame update
    void Start()
    {
        epicMusicSource.loop = true;
        epicMusicSource.playOnAwake = false;
        bossaMusicSource.loop = true;
        bossaMusicSource.playOnAwake = false;

        ambSource.loop = true;
        ambSource.playOnAwake = false;

        epicMusicSource.clip = epicMusic;
        epicMusicSource.volume = 0f;
        epicMusicSource.Play();

        bossaMusicSource.clip = bossaMusic;
        bossaMusicSource.volume = 0f;
        bossaMusicSource.Play();

        ambSource.clip = bed;
        ambSource.Play();
    }
    

    public void EpicToBossaMusic()
    {
        StartCoroutine(ETBCrossFade(0.05f));
    }

    IEnumerator ETBCrossFade(float factor)
    {
        if(epicMusicSource.volume > 0)
        {
            for(float v = 1f; v >=0; v -= factor)
            {
                epicMusicSource.volume = v;
                bossaMusicSource.volume = (1 - v);
                yield return new WaitForSeconds(factor);
            }
        }
    }

    public void BossaToEpicMusic()
    {
        StartCoroutine(BTECrossFade(0.05f));
    }

    IEnumerator BTECrossFade(float factor)
    {
        if (bossaMusicSource.volume > 0)
        {
            for (float v = 1f; v >= 0; v -= factor)
            {
                bossaMusicSource.volume = v;
                epicMusicSource.volume = (1 - v);
                yield return new WaitForSeconds(factor);
            }
        }
    }

    public void GameOverSound()
    {
        StartCoroutine(EFadeout(0.05f));
        interfaceSounds.clip = gameOverSound;
        interfaceSounds.Play();
    }
    public void WinSound()
    {
        StartCoroutine(EFadeout(0.05f));
        interfaceSounds.clip = winGameSound;
        interfaceSounds.Play();
    }

    IEnumerator EFadeout(float factor)
    {
        if (epicMusicSource.volume > 0)
        {
            for (float v = 1f; v >= 0; v -= factor)
            {
                epicMusicSource.volume = v;
                yield return new WaitForSeconds(factor);
            }
        }
    }
}
