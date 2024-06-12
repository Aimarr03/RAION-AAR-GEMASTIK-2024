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
        EnemyBase.OnEncounter += EnemyBase_OnEncounter;
        AudioManager.Instance.OnInstantStartNewMusic(normalBGM, 6f);
    }
    private void OnDisable()
    {
        EnemyBase.OnEncounter -= EnemyBase_OnEncounter;
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
}
