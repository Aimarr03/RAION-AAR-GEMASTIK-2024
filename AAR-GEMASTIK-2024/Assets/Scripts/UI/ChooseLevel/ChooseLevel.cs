using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseLevel : BasePreparingPlayerUI
{
    [SerializeField] private Transform mainPanel;
    private void Awake()
    {
        
    }
    
    public override IEnumerator OnEnterState()
    {
        gameObject.SetActive(true);
        mainPanel.gameObject.SetActive(true);
        GetComponent<RectTransform>().DOAnchorPosX(0, 0.7f);
        yield return null;
    }

    public override IEnumerator OnExitState()
    {
        GetComponent<RectTransform>().DOAnchorPosX(1000, 0.7f);
        yield return new WaitForSeconds(0.8f);
        mainPanel.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void OnChooseLevel(int level)
    {

    }
    
}
