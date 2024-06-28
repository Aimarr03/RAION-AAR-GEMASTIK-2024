using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseDataPlayerUI : MonoBehaviour
{
    [SerializeField] private List<SustainabilityShopUI> sustainabilityPlayerUIList;
    [SerializeField] private RectTransform itemUI;
    [SerializeField] private RectTransform weaponUI;
    [SerializeField] private RectTransform weightUI;
    [SerializeField] private RectTransform playerIcon;

    private void Start()
    {
        PreparingUIManager.OnChangeUI += PreparingUIManager_OnChangeUI;
        foreach (SustainabilityShopUI currentUI in sustainabilityPlayerUIList)
        {
            if (currentUI.sustainabilitySO.sustainabilityType == SustainabilityType.Capacity)
            {
                weightUI = currentUI.GetComponent<RectTransform>();
                break;
            }
        }
        playerIcon.DORotate(new Vector3(0, 360, 0), 7.6f, RotateMode.WorldAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    private void PreparingUIManager_OnChangeUI(bool isSetUpUI)
    {
        Debug.Log(isSetUpUI);
        if(isSetUpUI) CaseSetUpUI();
        else CaseNotSetUpUI();
    }
    private void CaseSetUpUI()
    {

        weightUI.DOAnchorPosX(-450, 0.75f).SetEase(Ease.InBack);
        itemUI.DOAnchorPosX(0, 0.75f).SetEase(Ease.OutBack).SetDelay(0.4f);
        weaponUI.DOAnchorPosY(-125, 0.75f).SetEase(Ease.OutBack).SetDelay(0.4f);
    }
    private void CaseNotSetUpUI()
    {
        itemUI.DOAnchorPosX(-450, 0.75f).SetEase(Ease.InBack);
        weaponUI.DOAnchorPosY(120, 0.75f).SetEase(Ease.InBack);
        weightUI.DOAnchorPosX(0, 0.75f).SetEase(Ease.OutBack).SetDelay(0.4f);
    }
}
