using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevel : BasePreparingPlayerUI
{
    [SerializeField] private Transform mainPanel;
    [SerializeField] private RectTransform Level1, Level2, Level3;
    [SerializeField] private TextMeshProUGUI levelUIText;
    private void Awake()
    {
        
    }
    
    public override IEnumerator OnEnterState()
    {
        mainPanel.gameObject.SetActive(true);
        GetComponent<RectTransform>().DOAnchorPosX(0, 0.7f);
        levelUIText.GetComponent<RectTransform>().DOAnchorPosY(5, 0.7f).SetEase(Ease.InOutQuint);
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnDisplay);
        levelUIText.text = $"{GameManager.Instance.currentLevelChoice}";
        GameManager.Instance.OnChangeLevelChoice += Instance_OnChangeLevelChoice;
        yield return null;
    }

    

    public override IEnumerator OnExitState()
    {
        GetComponent<RectTransform>().DOAnchorPosX(1000, 0.7f);
        AudioManager.Instance.PlaySFX(AudioContainerUI.instance.OnHide);
        GameManager.Instance.OnChangeLevelChoice -= Instance_OnChangeLevelChoice;
        levelUIText.GetComponent<RectTransform>().DOAnchorPosY(-50, 0.7f).SetEase(Ease.InOutQuint);
        yield return new WaitForSeconds(0.8f);
        mainPanel.gameObject.SetActive(false);
    }
    public void OnChooseLevel(int level)
    {
        levelUIText.text = $"Level : {level}";
    }
    private void Instance_OnChangeLevelChoice(string index)
    {
        levelUIText.text = $"{GameManager.Instance.currentLevelChoice}";
    }
    
}
