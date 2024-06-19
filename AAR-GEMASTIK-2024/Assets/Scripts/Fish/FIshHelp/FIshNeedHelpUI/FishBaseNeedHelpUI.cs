using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishBaseNeedHelpUI : MonoBehaviour
{
    public FishBaseNeedHelp fishBaseNeedHelp;    
    void Start()
    {
        fishBaseNeedHelp.OnGettingHelp += FishBaseNeedHelp_OnGettingHelp;
        fishBaseNeedHelp.OnBeingNoticed += FishBaseNeedHelp_OnBeingNoticed;
    }


    private void OnDisable()
    {
        fishBaseNeedHelp.OnGettingHelp -= FishBaseNeedHelp_OnGettingHelp;
        fishBaseNeedHelp.OnBeingNoticed -= FishBaseNeedHelp_OnBeingNoticed;
    }
    public void Unsubscribe()
    {
        fishBaseNeedHelp.OnGettingHelp -= FishBaseNeedHelp_OnGettingHelp;
        fishBaseNeedHelp.OnBeingNoticed -= FishBaseNeedHelp_OnBeingNoticed;
    }
    protected abstract void FishBaseNeedHelp_OnGettingHelp();
    protected abstract void FishBaseNeedHelp_OnBeingNoticed();
}
