using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource EffectSource;
    public static AudioManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void StartNewMusic(AudioClip clip)
    {
        MusicSource.Stop();
        MusicSource.clip = clip;
        MusicSource.Play();
        MusicSource.volume = 0;
        while(MusicSource.volume < 1)
        {
            MusicSource.volume = Mathf.Lerp(0, 1, 2f);
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        Debug.Log("PLAY SFX " + clip);
        SFXSource.PlayOneShot(clip);
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
