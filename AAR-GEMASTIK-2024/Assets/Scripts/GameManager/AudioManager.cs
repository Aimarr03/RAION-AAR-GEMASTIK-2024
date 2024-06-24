using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource EffectSource;
    [SerializeField] private AudioSource UnderwaterMusicSource;
    public static AudioManager Instance;
    private AudioData music, sfx, underwatersfx, additionaleffect;
    private float originalVolumeMusic;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        music = new AudioData(MusicSource, MusicSource.volume);
        sfx = new AudioData(SFXSource, SFXSource.volume);
        underwatersfx = new AudioData(UnderwaterMusicSource, UnderwaterMusicSource.volume);
        additionaleffect = new AudioData(EffectSource, EffectSource.volume);
    }
    private void Start()
    {
        OnGraduallyStartUnderwaterSFX(1.2f);
    }
    public void OnGraduallyStartUnderwaterSFX(float duration)
    {
        UnderwaterMusicSource.volume = 0;
        StartCoroutine(OnStartNewMusicGradually(underwatersfx, duration));
    }
    public void OnGraduallyStopUnderwaterSFX(float duration)
    {
        StartCoroutine(OnStopOldMusicGradually(underwatersfx, duration));
    }
    public void OnInstantStartNewMusic(AudioClip clip, float startDuration)
    {
        MusicSource.Stop();
        MusicSource.volume = 0;
        MusicSource.clip = clip;
        MusicSource.Play();
        StartCoroutine(OnStartNewMusicGradually(music, startDuration));
    }
    public void StartNewMusic(AudioClip clip, float stopDuration, float startDuration)
    {
        StartCoroutine(StartNewMusicCoroutine(clip, stopDuration, startDuration));
    }
    public void StopMusic(float duration)
    {
        StartCoroutine(StopOldMusicSourceGradually(duration));
    }
    private IEnumerator StopOldMusicSourceGradually(float duration)
    {
        float volume = MusicSource.volume;
        float elapseTime = 0;
        float maxDuration = duration;
        while (elapseTime < maxDuration)
        {
            elapseTime += Time.deltaTime;
            MusicSource.volume = Mathf.Lerp(volume, 0, elapseTime/maxDuration);
            yield return null;
        }
        MusicSource.volume = 0;
        MusicSource.Stop();
    }
    private IEnumerator StartNewMusicCoroutine(AudioClip clip, float stopDuration, float startDuration)
    {
        yield return OnStopOldMusicGradually(music,stopDuration);
        MusicSource.clip = clip;
        yield return OnStartNewMusicGradually(music, startDuration);
    }
    private IEnumerator OnStopOldMusicGradually(AudioData data, float stopDuration)
    {
        float volume = data.original_volume;
        float elapsedTime = 0;
        while(elapsedTime < stopDuration)
        {
            elapsedTime += Time.deltaTime;
            data.audioSource.volume = Mathf.Lerp(volume, 0, elapsedTime / stopDuration);
            yield return null;
        }
        data.audioSource.volume = 0;
        data.audioSource.Stop();
    }
    private IEnumerator OnStartNewMusicGradually(AudioData data,float startDuration)
    {
        data.audioSource.Play();
        float volume = 0f;
        float elapsedTime = 0;
        while(elapsedTime < startDuration)
        {
            elapsedTime += Time.deltaTime;
            data.audioSource.volume = Mathf.Lerp(volume, data.original_volume, elapsedTime / startDuration);
            yield return null;
        }
        data.audioSource.volume = data.original_volume;
    }
    public void PlaySFX(AudioClip clip)
    {
        Debug.Log("PLAY SFX " + clip);
        SFXSource.PlayOneShot(clip);
    }
    public void PlaySFX(AudioClip clip, float volume)
    {
        Debug.Log("PLAY SFX " + clip);
        SFXSource.PlayOneShot(clip, volume);
    }
    public void StartEffectSource()
    {
        EffectSource.Play();
    }
    public void StopEffectSource()
    {
        EffectSource.Stop();
    }
    public void StartNewEffectSource(AudioClip clip)
    {
        EffectSource.clip = clip;
        EffectSource.Play();
    }
}
[Serializable]
public class AudioData
{
    public AudioSource audioSource;
    public float original_volume;
    public AudioData(AudioSource audioSource, float original_volume)
    {
        this.audioSource = audioSource;
        this.original_volume = original_volume;
    }
}
