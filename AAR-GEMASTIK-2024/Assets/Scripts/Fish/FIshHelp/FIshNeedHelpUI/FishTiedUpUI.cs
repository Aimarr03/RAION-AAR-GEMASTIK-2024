using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishTiedUpUI : FishBaseNeedHelpUI
{
    private FishTiedUp tiedUpFish; 
    public Image foregroundProgress;
    public RectTransform backgroundProgress;
    private void Awake()
    {
        tiedUpFish = fishBaseNeedHelp as FishTiedUp;
        backgroundProgress.gameObject.SetActive(false);
    }
    protected override void FishBaseNeedHelp_OnGettingHelp()
    {
        if (!backgroundProgress.gameObject.activeSelf)
        {
            backgroundProgress.gameObject.SetActive(true);
        }
        foregroundProgress.fillAmount = tiedUpFish.percentageDuration;
        if(tiedUpFish.percentageDuration >= 1)
        {
            backgroundProgress.gameObject.SetActive(false);
            Unsubscribe();
        }
    }

    protected override void FishBaseNeedHelp_OnBeingNoticed()
    {
        Debug.Log("On Being Noticed");
        backgroundProgress.gameObject.SetActive(!tiedUpFish.playerIsNull);
    }
}
