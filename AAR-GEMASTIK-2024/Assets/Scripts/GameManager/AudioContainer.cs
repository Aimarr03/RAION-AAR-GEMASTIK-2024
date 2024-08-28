using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioContainer : MonoBehaviour
{
    public AudioClip normalBGM;
    public AudioClip fightBGM;
    public AudioClip gameOverBGM;

    public AudioClip AlertSFX;
    private bool IsFighting;
    private int manyEncounter = 0;
    private void Start()
    {
        IsFighting = false;
        AudioManager.Instance.OnInstantStartNewMusic(normalBGM, 3f);
        SharkBase.OnEncounter += SharkBase_OnEncounter;
        SharkBase.OnNeutralized += SharkBase_OnNeutralized;
        SharkBase.OnStopEncounter += SharkBase_OnStopEncounter;
        
        if(ExpedictionManager.Instance != null ) ExpedictionManager.Instance.OnLose += Instance_OnLose;
        
    }

    

    private void OnDisable()
    {
        SharkBase.OnEncounter -= SharkBase_OnEncounter;
        SharkBase.OnNeutralized -= SharkBase_OnNeutralized;
        SharkBase.OnStopEncounter -= SharkBase_OnStopEncounter;
        if (ExpedictionManager.Instance != null) ExpedictionManager.Instance.OnLose -= Instance_OnLose;
    }
    private void SharkBase_OnNeutralized()
    {
        if(manyEncounter > 0)
        {
            manyEncounter--;
        }
        if (manyEncounter == 0)
        {
            AudioManager.Instance?.StartNewMusic(normalBGM, 1f, 2f);
        }
    }
    private void SharkBase_OnStopEncounter()
    {
        if(manyEncounter > 0)
        {
            manyEncounter--;
        }
        if(manyEncounter == 0)
        {
            AudioManager.Instance?.StartNewMusic(normalBGM, 1f, 2f);
        }
    }
    private void SharkBase_OnEncounter()
    {
        if(manyEncounter == 0)
        {
            AudioManager.Instance?.PlaySFX(AlertSFX);
            AudioManager.Instance?.StartNewMusic(fightBGM, 1f, 2f);
        }
        manyEncounter++;
    }

    

    
    private void Instance_OnLose(string obj)
    {
        AudioManager.Instance.StartNewMusic(gameOverBGM, 1f, 2f);
    }
}
