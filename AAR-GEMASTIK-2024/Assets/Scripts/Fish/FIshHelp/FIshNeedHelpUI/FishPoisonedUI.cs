using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPoisonedUI : FishBaseNeedHelpUI
{
    private FishPoisoned fishPoisoned;
    public RectTransform VisualHelp;
    private void Awake()
    {
        fishPoisoned = fishBaseNeedHelp as FishPoisoned;
        VisualHelp.gameObject.SetActive(false);
    }
    protected override void FishBaseNeedHelp_OnBeingNoticed()
    {
        VisualHelp.gameObject.SetActive(!fishPoisoned.playerIsNull);
    }

    protected override void FishBaseNeedHelp_OnGettingHelp()
    {
        VisualHelp.gameObject.SetActive(false);
    }

}
