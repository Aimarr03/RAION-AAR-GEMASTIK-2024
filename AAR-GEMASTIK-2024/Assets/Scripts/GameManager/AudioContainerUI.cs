using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioContainerUI : MonoBehaviour
{
    public AudioClip normalBGM;
    public AudioClip interractable;
    public AudioClip uninterractable;
    public AudioClip OnTransaction;
    public AudioClip OnDisplay;
    public AudioClip OnHide;
    public AudioClip OnPop;

    public static AudioContainerUI instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        AudioManager.Instance.OnInstantStartNewMusic(normalBGM, 1.2f);
    }
}
