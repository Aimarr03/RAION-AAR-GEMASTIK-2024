using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SustainabilityShopUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textInfo;
    public SustainabilitySystemSO sustainabilitySO;

    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        DetailedCardView.OnUpgradedSomething += DetailedCardView_OnUpgradedSomething;
        RectTransform recttransform = GetComponent<RectTransform>();
        float local_y_value = recttransform.anchoredPosition.y;
        //Debug.Log(local_y_value);
        //Debug.Log(recttransform.anchoredPosition);
        //recttransform.DOAnchorPosY(10 + local_y_value, 0f);
        recttransform.DOAnchorPosY(-10 + local_y_value, Random.Range(0.9f,1.7f)).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        SetTextInfo();
    }

    private void DetailedCardView_OnUpgradedSomething()
    {
        SetTextInfo();
    }

    private void SetTextInfo()
    {
        int value = sustainabilitySO.maxValueTimesLevel;
        textInfo.text = $"{value}/{value}";
    }
}
