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
    private void Start()
    {
        IsFighting = false;
        AudioManager.Instance.OnInstantStartNewMusic(normalBGM, 3f);
        
        EnemyBase.OnEncounter += EnemyBase_OnEncounter;
        if(ExpedictionManager.Instance != null ) ExpedictionManager.Instance.OnLose += Instance_OnLose;
        
    }
    private void OnDisable()
    {
        EnemyBase.OnEncounter -= EnemyBase_OnEncounter;
        if (ExpedictionManager.Instance != null) ExpedictionManager.Instance.OnLose -= Instance_OnLose;
    }

    private void EnemyBase_OnEncounter()
    {
        if (IsFighting) return;
        IsFighting = true;
        if (IsFighting) 
        {
            AudioManager.Instance.PlaySFX(AlertSFX);
            AudioManager.Instance.StartNewMusic(fightBGM, 2f, 4f);
        }
    }
    private void Instance_OnLose(SustainabilityType obj)
    {
        AudioManager.Instance.StartNewMusic(gameOverBGM, 1f, 2f);
    }
}
