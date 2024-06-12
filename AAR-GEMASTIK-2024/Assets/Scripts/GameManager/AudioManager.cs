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

    private float originalVolumeMusic;
    private float originalVolumeSFX;
    private float originalVolumeEffect;
    private float originalVolumeUnderwaterEffect;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        originalVolumeMusic = MusicSource.volume;
        originalVolumeSFX = SFXSource.volume;
        originalVolumeEffect = EffectSource.volume;
        originalVolumeUnderwaterEffect = UnderwaterMusicSource.volume;
    }
    private void Start()
    {
        
        OnGraduallyStartUnderwaterSFX(1.2f);
    }
    public void OnGraduallyStartUnderwaterSFX(float duration)
    {
        UnderwaterMusicSource.volume = 0;
        StartCoroutine(OnStartNewMusicGradually(UnderwaterMusicSource, duration));
    }
    public void OnGraduallyStopUnderwaterSFX(float duration)
    {
        StartCoroutine(OnStopOldMusicGradually(UnderwaterMusicSource, duration));
    }
    public void OnInstantStartNewMusic(AudioClip clip, float startDuration)
    {
        MusicSource.Stop();
        MusicSource.volume = 0;
        MusicSource.clip = clip;
        MusicSource.Play();
        StartCoroutine(OnStartNewMusicGradually(MusicSource, startDuration));
    }
    public void StartNewMusic(AudioClip clip, float stopDuration, float startDuration)
    {
        StartCoroutine(StartNewMusicCoroutine(clip, stopDuration, startDuration));
    }
    private IEnumerator StartNewMusicCoroutine(AudioClip clip, float stopDuration, float startDuration)
    {
        yield return OnStopOldMusicGradually(MusicSource,stopDuration);
        MusicSource.clip = clip;
        yield return OnStartNewMusicGradually(MusicSource, startDuration);
    }
    private IEnumerator OnStopOldMusicGradually(AudioSource source, float stopDuration)
    {
        float volume = originalVolumeMusic;
        float elapsedTime = 0;
        while(elapsedTime < stopDuration)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(volume, 0, elapsedTime / stopDuration);
            yield return null;
        }
        source.volume = 0;
        source.Stop();
    }
    private IEnumerator OnStartNewMusicGradually(AudioSource source,float startDuration)
    {
        source.Play();
        float volume = 0f;
        float elapsedTime = 0;
        while(elapsedTime < startDuration)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(volume, originalVolumeMusic, elapsedTime / startDuration);
            yield return null;
        }
        source.volume = originalVolumeMusic;
    }
    public void PlaySFX(AudioClip clip)
    {
        Debug.Log("PLAY SFX " + clip);
        SFXSource.PlayOneShot(clip);
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
