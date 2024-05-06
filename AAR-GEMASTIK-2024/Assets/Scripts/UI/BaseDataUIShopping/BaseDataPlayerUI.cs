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

    private void Start()
    {
        PreparingUIManager.OnChangeUI += PreparingUIManager_OnChangeUI;
        
    }

    private void PreparingUIManager_OnChangeUI(bool isSetUpUI)
    {
        Debug.Log(isSetUpUI);
        if(isSetUpUI) CaseSetUpUI();
        else CaseNotSetUpUI();
    }
    private void CaseSetUpUI()
    {
        Transform capacitySustainabilityUI = null;
        foreach(SustainabilityShopUI currentUI in sustainabilityPlayerUIList)
        {
            if(currentUI.sustainabilitySO.sustainabilityType == SustainabilityType.Capacity)
            {
                capacitySustainabilityUI = currentUI.transform;
                break;
            }
        }
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        if(capacitySustainabilityUI != null)
        {
            sequence.Append(capacitySustainabilityUI.DOLocalMoveX(-450, 0.75f).SetEase(Ease.InBack));
        }
        itemUI.DOAnchorPosX(0, 0.75f).SetEase(Ease.OutBack).SetDelay(0.4f);
        weaponUI.DOAnchorPosY(-60, 0.75f).SetEase(Ease.OutBack).SetDelay(0.4f);
    }
    private void CaseNotSetUpUI()
    {

    }
}
