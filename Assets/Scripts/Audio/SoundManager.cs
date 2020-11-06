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

    /*[Header("Ambience")]

    public AudioSource ambSource;
    public AudioClip bed;
    public AudioClip[] emitters;*/

    [Header("UI")]

    public AudioSource interfaceSounds;
    //public AudioClip timer;
    public AudioClip gameOverSound;
    public AudioClip winGameSound;
    //public AudioClip startGameSound;
    public AudioClip tapeUp;
    public AudioClip tapeDown;


    void Start()
    {
        epicMusicSource.loop = true;
        epicMusicSource.playOnAwake = false;
        bossaMusicSource.loop = true;
        bossaMusicSource.playOnAwake = false;

        //ambSource.loop = true;
        //ambSource.playOnAwake = false;

        epicMusicSource.clip = epicMusic;
        epicMusicSource.volume = 0f;
        epicMusicSource.Play();

        bossaMusicSource.clip = bossaMusic;
        bossaMusicSource.volume = 0f;
        bossaMusicSource.Play();

        //ambSource.clip = bed;
        //ambSource.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            IntroEpic();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            EpicToBossaMusic();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            BossaToEpicMusic();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            GameOverSound();
        }
        if(Input.GetKeyDown(KeyCode.F5))
        {
            WinSound();
        }

    }

    public void IntroEpic()
    {
        StartCoroutine(StartFade(epicMusicSource, 2f, 1f));
    }

    public void EpicToBossaMusic()
    {
        StartCoroutine(StartFade(bossaMusicSource, 1f, 1f));
        StartCoroutine(StartFade(epicMusicSource, 1f, 0f));
        interfaceSounds.clip = tapeDown;
        interfaceSounds.Play();
    }

    public void BossaToEpicMusic()
    {
        StartCoroutine(StartFade(epicMusicSource, 1f, 1f));
        StartCoroutine(StartFade(bossaMusicSource, 1f, 0f));
        interfaceSounds.clip = tapeUp;
        interfaceSounds.Play();
    }

    public void GameOverSound()
    {
        StartCoroutine(StartFade(epicMusicSource, 2f, 0f));
        interfaceSounds.clip = gameOverSound;
        interfaceSounds.Play();
    }
    public void WinSound()
    {
        StartCoroutine(StartFade(epicMusicSource, 2f, 0f));
        interfaceSounds.clip = winGameSound;
        interfaceSounds.Play();
    }
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
